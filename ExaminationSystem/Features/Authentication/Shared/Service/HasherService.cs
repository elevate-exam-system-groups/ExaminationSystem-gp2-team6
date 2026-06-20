using System.Security.Cryptography;
using System.Text;
using ExaminationSystem.Contracts.Hasher;

namespace ExaminationSystem.Features.Authentication.Shared.Service;

public class HasherService : IHasherService
{
    public string Hash(string token)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = sha256.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }
}