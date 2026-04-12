using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExaminationSystem.Contracts.Token;
using ExaminationSystem.Domain.Entities.User.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ExaminationSystem.Features.Authentication.Token.Service;

public class AccessTokenService : IGenerateTokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AccessTokenService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
        };
        
        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var secretKey = _configuration.GetSection("JwtOptions").GetValue<string>("SecretKey");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtOptions:Issuer"],     // Token issuer
            audience: _configuration["JwtOptions:Audience"], // Token audience
            expires: DateTime.UtcNow.AddMinutes(15),         // Expiration time
            claims: claims,                                  // Payload (user info + roles)
            signingCredentials: credential                   // Signature (Header + Key)
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}