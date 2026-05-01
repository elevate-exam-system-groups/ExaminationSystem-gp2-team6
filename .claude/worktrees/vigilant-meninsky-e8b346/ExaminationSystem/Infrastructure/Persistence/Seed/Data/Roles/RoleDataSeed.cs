namespace ExaminationSystem.Infrastructure.Persistence.Seed.Data.Roles;

public class RoleDataSeed
{
    public static List<string> Roles => new()
    {
        "Admin",
        "Student"
    };
}