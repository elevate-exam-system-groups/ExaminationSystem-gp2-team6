using ExaminationSystem.Domain.Entities.Shared;

namespace ExaminationSystem.Domain.Entities.User
{
    public class LoginLog : BaseEntity<int>
    {
        public Guid UserId { get; set; }
        public DateTime LoggedInAt { get; set; } = DateTime.UtcNow;

        public User.ApplicationUser User { get; set; } = null!;
    }
}
