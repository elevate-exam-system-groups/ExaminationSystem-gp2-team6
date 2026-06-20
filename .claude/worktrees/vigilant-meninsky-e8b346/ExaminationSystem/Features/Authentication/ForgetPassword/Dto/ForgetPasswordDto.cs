namespace ExaminationSystem.Features.Authentication.ForgetPassword.Dto;

public class ForgetPasswordDto
{
    public Guid UserId { get; set; }
    
    public string Email { get; set; } = null!;
    
    public string Message { get; set; } = null!;
    
    public string OtpCode { get; set; } = null!;
}