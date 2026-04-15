using ExaminationSystem.Domain.Entities.Shared;

namespace ExaminationSystem.Domain.Entities.AttemptResult
{
    public class AttemptResult : BaseEntity<int>
    {
        public int AttemptId { get; set; }
        public int QuestionId { get; set; }

        public int? StudentAnswerOptionId { get; set; }   // what the student picked
        public int? CorrectAnswerOptionId { get; set; }   // the correct option

        public bool IsCorrect { get; set; }

        #region Relations

        public Attempt.Attempt Attempt { get; set; } = null!;
        public Question.Question Question { get; set; } = null!;

        #endregion
    }
}
