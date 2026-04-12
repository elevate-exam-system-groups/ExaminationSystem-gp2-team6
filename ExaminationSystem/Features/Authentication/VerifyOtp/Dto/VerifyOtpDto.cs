namespace ExaminationSystem.Features.Authentication.VerifyOtp.Dto;

public class VerifyOtpDto
{ 
    public string Email { get; set; } = null!;
    
    public string Token { get; set; } = null!;
    
    public DateTime ExpiresAt { get; set; }
    
    public string Message { get; set; } = null!;
}