using ExaminationSystem.Domain.Entities.Shared;
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

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}
