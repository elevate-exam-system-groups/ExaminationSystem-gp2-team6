namespace ExaminationSystem.Features.Authentication.SendOtp.Dto;

public class SendOtpDto
{
    public string Email { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string OtpCode { get; set; } = null!;
}