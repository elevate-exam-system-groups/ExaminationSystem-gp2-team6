namespace ExaminationSystem.Features.Authentication.ResetPassword.Request;

public class ResetPasswordRequest
{
    public Guid UserId { get; set; }

    public string Token { get; set; } = null!;
    
    public string Password { get; set; } = null!;
    
    public string ConfirmedPassword { get; set; } = null!;
}