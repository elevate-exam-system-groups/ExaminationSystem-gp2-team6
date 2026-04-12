using ExaminationSystem.Domain.Entities.Shared.Enums.User;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Domain.Entities.User;

public class ApplicationUser: IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FullName { get; set; } = null!; // Computed Column FirstName + LastName
    
    public string? ProfileImageUrl { get; set; }
    
    public UserType UserType { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}