using ExaminationSystem.Domain.Entities.Diploma;
using ExaminationSystem.Domain.Entities.Shared.Enums.User;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Domain.Entities.User;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FullName { get; set; } = null!; // Computed Column FirstName + LastName

    public string? ProfileImageUrl { get; set; }

    public UserType UserType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<StudentDiplomaEnrollment> StudentDiplomaEnrollments { get; set; } = new HashSet<StudentDiplomaEnrollment>();

    public ICollection<Domain.Entities.QuizAttempt.QuizAttempt> QuizAttempts { get; set; } = new HashSet<Domain.Entities.QuizAttempt.QuizAttempt>();
    public ICollection<LoginLog> LoginLogs { get; set; } = new HashSet<LoginLog>();
    public ICollection<UserRefreshToken.UserRefreshToken> RefreshTokens { get; set; } = new List <UserRefreshToken.UserRefreshToken>();

}