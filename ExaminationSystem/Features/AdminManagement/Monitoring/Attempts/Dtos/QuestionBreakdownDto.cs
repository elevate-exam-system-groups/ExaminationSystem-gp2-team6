namespace ExaminationSystem.Features.AdminManagement.Monitoring.Attempts.Dtos
{
    public class QuestionBreakdownDto
    {
        public string QuestionText { get; set; } = null!;
        public string? StudentAnswer { get; set; }
        public string CorrectAnswer { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public int PointsEarned { get; set; }
    }
}
