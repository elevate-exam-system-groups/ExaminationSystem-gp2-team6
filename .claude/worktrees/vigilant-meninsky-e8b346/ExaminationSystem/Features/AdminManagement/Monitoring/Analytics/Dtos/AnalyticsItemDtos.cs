namespace ExaminationSystem.Features.AdminManagement.Monitoring.Analytics.Dtos
{
    public record QuizPassRateDto(int QuizId, string QuizTitle, double PassRate);
    public record DiplomaAvgScoreDto(int DiplomaId, string DiplomaTitle, double AverageScore);
    public record AttemptsOverTimeDto(string Date, int AttemptCount);
    public record TopFailedQuestionDto(int QuestionId, string QuestionText, double CorrectRate);
}
