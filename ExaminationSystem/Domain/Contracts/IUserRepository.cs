using ExaminationSystem.Domain.Entities.User;

namespace ExaminationSystem.Domain.Contracts
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(Guid id);
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<IReadOnlyList<ApplicationUser>> GetStudentsAsync();
    }
}
