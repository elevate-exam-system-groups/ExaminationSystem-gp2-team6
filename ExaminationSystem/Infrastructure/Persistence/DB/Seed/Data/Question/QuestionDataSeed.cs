namespace ExaminationSystem.Infrastructure.Persistence.DB.Seed.Data.Question;

public static class QuestionDataSeed
{
    public static List<Domain.Entities.Question.Question> Questions => new()
    {
        new Domain.Entities.Question.Question
        {
            Id = 1,
            QuestionText = "What is C#?",
            Explanation = "C# is a programming language developed by Microsoft.",
            QuizId = 1,
            OrderIndex = 1
        },
        new Domain.Entities.Question.Question
        {
            Id = 2,
            QuestionText = "What is .NET?",
            Explanation = ".NET is a development platform/framework.",
            QuizId = 2,
            OrderIndex = 1
        }
    };
}