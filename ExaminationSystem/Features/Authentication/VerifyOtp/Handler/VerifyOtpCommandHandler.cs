using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Contracts.Hasher;
using ExaminationSystem.Domain.Entities.PasswordResetTemporaryToken;
using ExaminationSystem.Domain.Entities.User.ApplicationUser;
using ExaminationSystem.Features.Authentication.VerifyOtp.Command;
using ExaminationSystem.Features.Authentication.VerifyOtp.Dto;
using ExaminationSystem.Infrastructure.Persistence.DB.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Authentication.VerifyOtp.Handler;

public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, RequestResult<VerifyOtpDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _dbContext;
    private readonly IHasherService _hasher;
    private readonly IHasherService _hasherService;
    
    public VerifyOtpCommandHandler(UserManager<ApplicationUser> userManager, AppDbContext dbContext, IHasherService hasherService, IHasherService hasher) {
        _userManager = userManager;
        _dbContext = dbContext;
        _hasherService = hasherService;
        _hasher = hasher;
    }
    
    public async Task<RequestResult<VerifyOtpDto>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var hashedCode = _hasherService.Hash(request.VerifyCode);

        var otp = await _dbContext.UserOtps
            .Where(x => x.UserId == request.UserId && !x.IsUsed)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.Code == hashedCode, cancellationToken);

        if (otp == null) return RequestResult<VerifyOtpDto>.Failure(ErrorCode.VerificationCodeInvalid, "Invalid code");
        if (otp.IsUsed) return RequestResult<VerifyOtpDto>.Failure(ErrorCode.VerificationCodeExpired, "Code already used");
        if (otp.ExpiresAt < DateTime.UtcNow) return RequestResult<VerifyOtpDto>.Failure(ErrorCode.VerificationCodeExpired, "Code expired");

        otp.IsUsed = true;

        var user = await _userManager.FindByIdAsync(otp.UserId.ToString());
        
        if (user == null) return RequestResult<VerifyOtpDto>.Failure(ErrorCode.UserNotFound, "User not found");

        var temporaryToken = Guid.NewGuid().ToString();
        var hashedToken = _hasher.Hash(temporaryToken);
        
        var passwordResettemporaryToken = new PasswordResetTemporaryToken()
        {
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            Token = hashedToken,
            IsUsed = false,
            IsDeleted = true,
        };

        await _dbContext.PasswordResetTemporaryTokens.AddAsync(passwordResettemporaryToken, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return RequestResult<VerifyOtpDto>.Success(new VerifyOtpDto
        {
            Email = user.Email!,
            Token = temporaryToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            Message = $"Successfully verified OTP Code"
        });
    }
}