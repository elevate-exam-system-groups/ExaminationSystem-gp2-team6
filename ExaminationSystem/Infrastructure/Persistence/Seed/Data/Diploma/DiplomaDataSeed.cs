using ExaminationSystem.Domain.Entities.Shared.Enums.Diploma;

namespace ExaminationSystem.Infrastructure.Persistence.Seed.Data.Diploma;

public static class DiplomaDataSeed
{
    public static List<Domain.Entities.Diploma.Diploma> Diplomas => new()
    {
        new Domain.Entities.Diploma.Diploma ()
        {
            Title = "C#",
            Description = "Learn C# from scratch",
            Status = DiplomaStatus.Published,
        },
        new Domain.Entities.Diploma.Diploma ()
        {
            Title = ".NET Diploma",
            Description = "Master .NET backend",
            Status = DiplomaStatus.Published,
        }
    };
}