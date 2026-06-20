namespace ExaminationSystem.Features.QuizEngine.ViewResults.Dtos
{
    public sealed record AttemptSummaryDto(
    int AttemptId,
    string QuizTitle,
    double? Score,
    bool? Passed,
    string Status,
    DateTime? SubmittedAt
);
}
