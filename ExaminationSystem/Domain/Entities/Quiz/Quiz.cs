using ExaminationSystem.Domain.Entities.Shared;
using ExaminationSystem.Domain.Entities.Shared.Enums.Quiz;

namespace ExaminationSystem.Domain.Entities.Quiz;

public class Quiz : BaseEntity<int>
{
    public string Title { get; set; } = null!;
    
    public TimeSpan Duration { get; set; }

    public int PassScore { get; set; } = 60;

    public int? MaxAttempts { get; set; }

    public QuizStatus Status { get; set; } = QuizStatus.Draft;
    
    public string? Instructions  { get; set; }

    #region Relations

    #region Dipolma

    public int DiplomaId { get; set; } // Foreign Key
    
    public Diploma.Diploma Diploma { get; set; } = null!; // Navigational Property

    #endregion
    
    #region Questions
    
    public ICollection <Question.Question> Questions { get; set; } = new List <Question.Question>(); // Navigational Property
    
    #endregion

    #endregion
}