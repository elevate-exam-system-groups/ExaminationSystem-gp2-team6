using ExaminationSystem.Domain.Entities.Shared;
using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;
using ExaminationSystem.Domain.Entities.User;

namespace ExaminationSystem.Domain.Entities.QuizAttempt
{
    public class QuizAttempt : BaseEntity<int>
    {
        public int QuizId { get; set; }
        public Guid StudentId { get; set; }  

        public AttemptStatus Status { get; set; } = AttemptStatus.InProgress;

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SubmittedAt { get; set; }

        // Calculated after submission
        public double? Score { get; set; }
        public bool? Passed { get; set; }

        #region Relations

        public Quiz.Quiz Quiz { get; set; } = null!;
        public ApplicationUser Student { get; set; } = null!;

        public ICollection<AttemptAnswer.AttemptAnswer> AttemptAnswers { get; set; }
            = new HashSet<AttemptAnswer.AttemptAnswer>();

        public ICollection<AttemptResult.AttemptResult> AttemptResults { get; set; }
            = new HashSet<AttemptResult.AttemptResult>();

        #endregion

        // Domain helper — has the timer expired?
        public bool IsTimedOut(TimeSpan quizDuration)
            => DateTime.UtcNow > StartedAt.Add(quizDuration);
    }
}
