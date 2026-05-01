using ExaminationSystem.Domain.Entities.Shared;

namespace ExaminationSystem.Domain.Entities.User.UserOtp;

public class UserOtp: BaseEntity<int>
{
    public string Code { get; set; } = null!; // hashed

    public DateTime ExpiresAt { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsUsed { get; set; } = false;

    public int Attempts { get; set; } = 0;

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
}