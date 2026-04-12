namespace ExaminationSystem.Features.Authentication.Login.Dto;

public class LoginDto
{
    public string AccessToken { get; set; } = null!;
    
    public string RefreshToken { get; set; } = null!;
}