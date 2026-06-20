namespace ExaminationSystem.Features.Authentication.Login.ViewModel;

public record LoginViewModel
{
    public string AccessToken { get; set; } = null!;
}