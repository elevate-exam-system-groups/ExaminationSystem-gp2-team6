using ExaminationSystem.Domain.Entities.Shared;
using ExaminationSystem.Domain.Entities.User.ApplicationUser;

namespace ExaminationSystem.Domain.Entities.PasswordResetTemporaryToken;

public class PasswordResetTemporaryToken : BaseEntity<int>
{
    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;
    
    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}