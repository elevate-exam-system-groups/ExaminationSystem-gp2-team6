namespace ExaminationSystem.Infrastructure.Persistence.DB.Seed.Data.AnswerOption;

public static class AnswerOptionDataSeed
{
    public static List<Domain.Entities.AnswerOption.AnswerOption> Answers => new( )
    {
        // Question 1 (C#)
        new Domain.Entities.AnswerOption.AnswerOption { Id = 1, Text = "Programming Language", IsCorrect = true, QuestionId = 1 },
        new Domain.Entities.AnswerOption.AnswerOption { Id = 2, Text = "Database System", IsCorrect = false, QuestionId = 1 },
        new Domain.Entities.AnswerOption.AnswerOption { Id = 3, Text = "Operating System", IsCorrect = false, QuestionId = 1 },
        new Domain.Entities.AnswerOption.AnswerOption { Id = 4, Text = "Web Browser", IsCorrect = false, QuestionId = 1 },

        // Question 2 (.NET)
        new Domain.Entities.AnswerOption.AnswerOption { Id = 5, Text = "Framework", IsCorrect = true, QuestionId = 2 },
        new Domain.Entities.AnswerOption.AnswerOption { Id = 6, Text = "Programming Language", IsCorrect = false, QuestionId = 2 },
        new Domain.Entities.AnswerOption.AnswerOption { Id = 7, Text = "Database Engine", IsCorrect = false, QuestionId = 2 },
        new Domain.Entities.AnswerOption.AnswerOption { Id = 8, Text = "Operating System", IsCorrect = false, QuestionId = 2 }
    };
}