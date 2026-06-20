namespace ExaminationSystem.Features.Authentication.VerifyOtp.VerifyOtpViewModel;

public class VerifyOtpViewModel
{
    public string Token { get; set; } = null!;
    
    public DateTime ExpiresAt { get; set; }
    
    public string Message { get; set; } = null!;
}