using ExaminationSystem.Domain.Entities.User;
using ExaminationSystem.Domain.Entities.User.ApplicationUser;

namespace ExaminationSystem.Contracts.Token;

public interface IGenerateTokenService
{
    public Task<string> GenerateTokenAsync(ApplicationUser user);
}