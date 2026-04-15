namespace ExaminationSystem.Features.QuizEngine.ViewResults.Dtos
{
    public sealed record QuestionResultDto(
    int QuestionId,
    string QuestionText,
    int? StudentAnswerOptionId,
    int? CorrectAnswerOptionId,
    bool IsCorrect,
    string? Explanation
    );
}
