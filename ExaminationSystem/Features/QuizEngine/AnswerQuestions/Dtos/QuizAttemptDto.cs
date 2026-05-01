using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Dtos
{
    public class QuizAttemptDto
    {
        public int AttemptId { get; set; }
        public int QuizId { get; set; }
        public Guid StudentId { get; set; }
        public double? Score { get; set; }
        public AttemptStatus Status { get; set; }
        public DateTime StartedAt { get; set; }
        public int Duration { get; set; }


    }
}