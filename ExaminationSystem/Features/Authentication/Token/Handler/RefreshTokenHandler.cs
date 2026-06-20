using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Contracts.Hasher;
using ExaminationSystem.Contracts.Token;
using ExaminationSystem.Domain.Entities.User.UserRefreshToken;
using ExaminationSystem.Features.Authentication.Token.Command;
using ExaminationSystem.Features.Authentication.Token.Dto;
using ExaminationSystem.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Authentication.Token.Handler;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, RequestResult<TokensDto>>
{
    private readonly AppDbContext _context;
    private readonly IGenerateTokenService _tokenService;
    private readonly IGenerateRefreshTokenService _refreshTokenService;
    private readonly IHasherService _hasher;

    public RefreshTokenHandler(AppDbContext context, IGenerateTokenService tokenService, IHasherService hasher, IGenerateRefreshTokenService refreshTokenService)
    {
        _context = context;
        _tokenService = tokenService;
        _hasher = hasher;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<RequestResult<TokensDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var hashedToken = _hasher.Hash(request.RefreshToken);

        var storedToken = await _context.UserRefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(x => x.Token == hashedToken && !x.IsRevoked, cancellationToken);
        
        if (storedToken == null) return RequestResult<TokensDto>.Failure(ErrorCode.Unauthorized, "Invalid refresh token");

        if (storedToken.ExpiresAt < DateTime.UtcNow) return RequestResult<TokensDto>.Failure(ErrorCode.Unauthorized, "Token expired");
        
        var user = storedToken.User;

        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow) return RequestResult<TokensDto>.Failure(ErrorCode.UserLocked, "User is locked");

        // revoke old token
        storedToken.IsRevoked = true;

        // generate new refresh token
        var newRefreshToken = _refreshTokenService.GenerateRefreshToken();
        var hashedNewToken = _hasher.Hash(newRefreshToken);

        var newToken = new UserRefreshToken
        {
            Token = hashedNewToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        await _context.UserRefreshTokens.AddAsync(newToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // new access token
        var newAccessToken = await _tokenService.GenerateTokenAsync(user);

        var dto = new TokensDto()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };

        return RequestResult<TokensDto>.Success(dto, "Token refreshed");
    }
}