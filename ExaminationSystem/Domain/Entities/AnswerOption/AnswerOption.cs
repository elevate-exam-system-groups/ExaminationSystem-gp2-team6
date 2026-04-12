using ExaminationSystem.Domain.Entities.Shared;

namespace ExaminationSystem.Domain.Entities.AnswerOption;

public class AnswerOption : BaseEntity<int>
{
    public string Text { get; set; } = null!;
    
    public bool IsCorrect { get; set; }

    #region Relations
    
    public int QuestionId { get; set; } // Foreign Key
    
    public Question.Question Question { get; set; } = null!; // Navigational Property
    
    #endregion
}