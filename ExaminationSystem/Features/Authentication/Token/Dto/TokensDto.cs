namespace ExaminationSystem.Features.Authentication.Token.Dto;

public class TokensDto
{
    public string AccessToken { get; set; } = null!;
    
    public string RefreshToken { get; set; } = null!;
}