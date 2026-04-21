using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Shared.Enums.User;
using ExaminationSystem.Domain.Entities.User;
using ExaminationSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser?> GetByIdAsync(Guid id) =>
             await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);


        public async Task<ApplicationUser?> GetByEmailAsync(string email) =>
             await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);


        public async Task<IReadOnlyList<ApplicationUser>> GetStudentsAsync() =>
             await _context.Users
                .Where(u => u.UserType == UserType.Student)
                .ToListAsync();


        public async Task<int> CountAsync()
             => await _context.Users.CountAsync();
    }
}
