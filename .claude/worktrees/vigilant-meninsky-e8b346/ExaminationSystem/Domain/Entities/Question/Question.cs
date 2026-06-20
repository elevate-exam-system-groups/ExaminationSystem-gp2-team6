using ExaminationSystem.Domain.Entities.Shared;

namespace ExaminationSystem.Domain.Entities.Question;

public class Question : BaseEntity<int>
{
    public string QuestionText { get; set; } = null!;
    
    public string? Explanation { get; set; }

    public int OrderIndex { get; set; }

    #region Relations

    #region Quiz

    public int QuizId { get; set; } // Foreign Key
    
    public Quiz.Quiz Quiz { get; set; } = null!; // Navigational Property

    #endregion

    #region QuestionOption
    
    public ICollection<AnswerOption.AnswerOption> AnswerOptions { get; set; } = new HashSet<AnswerOption.AnswerOption>();
    
    #endregion

    #endregion
}