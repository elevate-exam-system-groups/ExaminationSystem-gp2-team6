using ExaminationSystem.Domain.Entities.Shared;
using ExaminationSystem.Domain.Entities.User;

namespace ExaminationSystem.Domain.Entities.Diploma;

public class StudentDiplomaEnrollment:BaseEntity<int>
{
    public Guid StudentId { get; set; }

    public ApplicationUser Student { get; set; } = null!;

    public int DiplomaId { get; set; }

    public Diploma Diploma { get; set; } = null!;

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
}
