using ExaminationSystem.Domain.Entities.Shared;
using ExaminationSystem.Domain.Entities.Shared.Enums.Diploma;

namespace ExaminationSystem.Domain.Entities.Diploma;

public class Diploma : BaseEntity<int>
{
    public string Title { get; set; } = null!;
    
    public string? Description { get; set; }

    public DiplomaStatus Status { get; set; } = DiplomaStatus.Draft;
    
    public int QuizCount => Quizzes.Count; // Computed Property
    
    #region Relations

    public ICollection <Quiz.Quiz> Quizzes { get; set; } = new HashSet <Quiz.Quiz>(); // Navigational Property
    
    #endregion
    
}