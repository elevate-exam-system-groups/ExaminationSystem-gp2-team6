using ExaminationSystem.Domain.Entities.Shared;
using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;
using ExaminationSystem.Domain.Entities.User;

namespace ExaminationSystem.Domain.Entities.Quiz;

public class QuizAttempt : BaseEntity<int>
{
    public Guid StudentId { get; set; }

    public ApplicationUser Student { get; set; } = null!;

    public int QuizId { get; set; }

    public Quiz Quiz { get; set; } = null!;

    /// <summary>Score percentage (0–100).</summary>
    public int Score { get; set; }
    /// <summary>Status: InProgress, Submitted</summary>
    public AttemptStatus Status { get; set; } = AttemptStatus.InProgress;

    /// <summary>When student started the quiz</summary>
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}
