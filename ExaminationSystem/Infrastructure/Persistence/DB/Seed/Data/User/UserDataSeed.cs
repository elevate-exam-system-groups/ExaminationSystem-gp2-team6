using ExaminationSystem.Domain.Entities.Shared.Enums.User;
using ExaminationSystem.Domain.Entities.User;

namespace ExaminationSystem.Infrastructure.Persistence.DB.Seed.Data.User;

public static class UserDataSeed
{
    public static List<ApplicationUser> Users => new ()
    {
        new ApplicationUser
        {
            Id = Guid.NewGuid(),
            FirstName = "Mohamed",
            LastName = "Elganzory",
            UserName = "admin@test.com",
            Email = "mohamedelganzory621@gmail.com",
            UserType = UserType.Admin,
            EmailConfirmed = true
        },
        new ApplicationUser
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Test",
            Email = "test@gmail.com",
            UserType = UserType.Student
        }
    };
}