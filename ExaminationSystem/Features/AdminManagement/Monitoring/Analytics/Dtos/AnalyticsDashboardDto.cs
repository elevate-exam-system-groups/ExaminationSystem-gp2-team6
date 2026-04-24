namespace ExaminationSystem.Features.AdminManagement.Monitoring.Analytics.Dtos
{
    public class AnalyticsDashboardDto
    {
        public List<QuizPassRateDto> PassRateByQuiz { get; set; } = new();
        public List<DiplomaAvgScoreDto> AvgScoreByDiploma { get; set; } = new();
        public List<AttemptsOverTimeDto> AttemptsOverTime { get; set; } = new();
        public List<TopFailedQuestionDto> TopFailedQuestions { get; set; } = new();
    }
}
