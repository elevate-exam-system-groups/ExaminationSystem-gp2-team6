using System.Security.Cryptography;
using ExaminationSystem.Contracts.Token;

namespace ExaminationSystem.Features.Authentication.Token.Service;

public class RefreshTokenService : IGenerateRefreshTokenService
{
    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }
}