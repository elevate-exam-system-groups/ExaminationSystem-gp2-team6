using ExaminationSystem.Domain.Entities.Shared.Enums.User;

namespace ExaminationSystem.Features.Authentication.Register.ViewModel;

public class RegisterViewModel
{
    public string Email { get; set; } = null!; 
    
    public string AccessToken { get; set; } = null!;
    
    public UserType UserType { get; set; } = UserType.Student;
}