namespace ExaminationSystem.Features.QuizEngine.ViewResults.Dtos
{
    public sealed record AttemptResultsDto(
    double Score,
    bool Passed,
    int TotalQuestions,
    int CorrectCount,
    IReadOnlyList<QuestionResultDto> PerQuestion
);
}
