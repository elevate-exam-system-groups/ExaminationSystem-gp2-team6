namespace ExaminationSystem.Domain.Entities.User.UserRefreshToken;

public class UserRefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public Guid UserId { get; set; }
    
    public ApplicationUser User { get; set; } = null!;
}