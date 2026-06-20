using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;

namespace ExaminationSystem.Features.AdminManagement.Monitoring.Attempts.Dtos
{
    public class AttemptDetailDto
    {
        public int AttemptId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string QuizTitle { get; set; } = null!;
        public int Score { get; set; }
        public AttemptStatus Status { get; set; }
        public DateTime SubmittedAt { get; set; }
        public List<QuestionBreakdownDto> QuestionBreakdowns { get; set; } = new();
    }
}
