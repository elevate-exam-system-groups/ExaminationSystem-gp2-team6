using ExaminationSystem.Domain.Entities.User;

namespace ExaminationSystem.Contracts.Token;

public interface IGenerateTokenService
{
    public Task<string> GenerateTokenAsync(ApplicationUser user);
}