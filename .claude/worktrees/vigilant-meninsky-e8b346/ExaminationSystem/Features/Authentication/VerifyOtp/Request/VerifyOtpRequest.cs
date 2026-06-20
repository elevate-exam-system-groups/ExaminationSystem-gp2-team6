namespace ExaminationSystem.Features.Authentication.VerifyOtp.Request;

public class VerifyOtpRequest
{
    public Guid UserId { get; set; }

    public string VerifyCode { get; set; } = null!;
}