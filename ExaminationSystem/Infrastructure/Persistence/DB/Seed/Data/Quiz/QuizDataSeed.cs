using ExaminationSystem.Domain.Entities.Shared.Enums.Quiz;

namespace ExaminationSystem.Infrastructure.Persistence.DB.Seed.Data.Quiz;

public static class QuizDataSeed
{
    public static List<Domain.Entities.Quiz.Quiz> Quizzes() => new()
    {
        new Domain.Entities.Quiz.Quiz
        {
            Title = "C# Basics",
            Duration = TimeSpan.FromMinutes(30),
            PassScore = 60,
            MaxAttempts = 3,
            Status = QuizStatus.Published,
            Instructions = "Answer all questions carefully",
            DiplomaId = 1
        },
        new Domain.Entities.Quiz.Quiz
        {
            Title = ".NET Basics",
            Duration = TimeSpan.FromMinutes(45),
            PassScore = 60,
            MaxAttempts = 3,
            Status = QuizStatus.Published,
            Instructions = "Choose the correct answer",
            DiplomaId = 2
        }
    };
    
}