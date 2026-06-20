using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Contracts.Hasher;
using ExaminationSystem.Contracts.Token;
using ExaminationSystem.Domain.Entities.User;
using ExaminationSystem.Domain.Entities.User.UserRefreshToken;
using ExaminationSystem.Features.Authentication.Login.Command;
using ExaminationSystem.Features.Authentication.Login.Dto;
using ExaminationSystem.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Authentication.Login.Handler;

public class LoginCommandHandler : IRequestHandler<LoginCommand, RequestResult<LoginDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGenerateTokenService _generateTokenService;
    private readonly IGenerateRefreshTokenService _refreshTokenService;
    private readonly AppDbContext _context;
    private readonly IHasherService _hasher;
    
    public LoginCommandHandler(UserManager<ApplicationUser> userManager, IGenerateTokenService generateTokenService, IGenerateRefreshTokenService refreshTokenService, AppDbContext context, IHasherService hasher)
    {
        _userManager = userManager;
        _generateTokenService = generateTokenService;
        _refreshTokenService = refreshTokenService;
        _context = context;
        _hasher = hasher;
    }
    
    public async Task<RequestResult<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        #region Email

        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null) return RequestResult<LoginDto>.Failure(ErrorCode.UserNotFound, "Invalid email or password");
        if (!user.EmailConfirmed) return RequestResult<LoginDto>.Failure(ErrorCode.Unauthorized, "Account is not verified");
        if (await _userManager.IsLockedOutAsync(user)) return RequestResult<LoginDto>.Failure(ErrorCode.TooManyRequests, "The User is locked. Try again after 15 Min.");

        #endregion

        #region Password

        var passwordIsValid = await _userManager.CheckPasswordAsync(user, request.Password);
        
        if (!passwordIsValid)
        {
            await _userManager.AccessFailedAsync(user); // LockedCounter ++
            return RequestResult<LoginDto>.Failure(ErrorCode.InvalidCredentials, "Invalid email or password");
        }
        
        await _userManager.ResetAccessFailedCountAsync(user); // Reset LockedCounter

        #endregion

        #region Token - Refresh Token

        var accessToken = await _generateTokenService.GenerateTokenAsync(user);
        var refreshToken = _refreshTokenService.GenerateRefreshToken();
        var hashedToken = _hasher.Hash(refreshToken);

        var refreshTokenEntity = new UserRefreshToken
        {
            Token = hashedToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        _context.UserRefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync(cancellationToken);

        #endregion

        #region DTO

        var dto = new LoginDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        #endregion
        
        return RequestResult<LoginDto>.Success(dto, "Success");
    }
}