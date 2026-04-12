using ExaminationSystem.Domain.Entities.Shared.Enums.User;
using ExaminationSystem.Domain.Entities.User;

namespace ExaminationSystem.Infrastructure.Persistence.DB.Seed.Data.User;

public static class UserDataSeed
{
    public static List<ApplicationUser> Users => new ()
    {
        new ApplicationUser
        {
            FirstName = "Mohamed",
            LastName = "Elganzory",
            UserName = "admin@test.com",
            Email = "mohamedelganzory621@gmail.com",
            UserType = UserType.Admin,
            EmailConfirmed = true
        },
        new ApplicationUser
        {
            FirstName = "Test",
            LastName = "Test",
            UserName = "test@gmail.com",
            Email = "test@gmail.com",
            EmailConfirmed = true,
            UserType = UserType.Student
        }
    };
}