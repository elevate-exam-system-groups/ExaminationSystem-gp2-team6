namespace ExaminationSystem.Features.Authentication.ForgetPassword.ViewModel;

public class ForgetPasswordViewModel
{
    public Guid UserId { get; set; }
    
    public string Email { get; set; } = null!;
    
    public string Message { get; set; } = null!;
}