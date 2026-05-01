namespace ExaminationSystem.Features.Authentication.VerifyOtp.VerifyOtpViewModel;

public class ResendOtpViewModel
{
    public string Email { get; set; } = null!;
    
    public string Token { get; set; } = null!;
    
    public string Message { get; set; } = null!;
}