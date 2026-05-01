using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Contracts.Hasher;
using ExaminationSystem.Domain.Entities.User;
using ExaminationSystem.Features.Authentication.ResetPassword.Command;
using ExaminationSystem.Features.Authentication.ResetPassword.Dto;
using ExaminationSystem.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Authentication.ResetPassword.Handler;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, RequestResult<ResetPasswordDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHasherService _hasher;
    private readonly AppDbContext _dbContext;

    public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager, IHasherService hasher, AppDbContext dbContext)
    {
        _userManager = userManager;
        _hasher = hasher;
        _dbContext = dbContext;
    }
    
    public async Task<RequestResult<ResetPasswordDto>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var hashedToken = _hasher.Hash(request.Token);

        var token = await _dbContext.PasswordResetTemporaryTokens.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.Token == hashedToken, cancellationToken);
        if (token == null || token.IsUsed || token.ExpiresAt < DateTime.UtcNow) return RequestResult<ResetPasswordDto>.Failure(ErrorCode.ExpiredTemporaryToken, "Invalid or expired token");

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) return RequestResult<ResetPasswordDto>.Failure(ErrorCode.UserNotFound, "User not found");

        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
        token.IsUsed = true;
    
        var refreshTokens = await _dbContext.UserRefreshTokens
            .Where(x => x.UserId == request.UserId && !x.IsRevoked)
            .ToListAsync(cancellationToken);

        refreshTokens.ForEach(rt => rt.IsRevoked = true);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        var dto = new ResetPasswordDto
        {
            Message = "Password reset successful"
        };

        return RequestResult<ResetPasswordDto>.Success(dto, "Success");
    }
}