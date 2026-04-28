using ExaminationSystem.Domain.Entities.Shared;

namespace ExaminationSystem.Domain.Entities.AttemptAnswer
{
    public class AttemptAnswer : BaseEntity<int>
    {
        public int AttemptId { get; set; }
        public int QuestionId { get; set; }
        public int? SelectedOptionId { get; set; }   // nullable → question skipped

        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;

        #region Relations

        public Domain.Entities.QuizAttempt.QuizAttempt QuizAttempt { get; set; } = null!;
        public Question.Question Question { get; set; } = null!;
        public AnswerOption.AnswerOption? SelectedOption { get; set; }

        #endregion
    }
}
