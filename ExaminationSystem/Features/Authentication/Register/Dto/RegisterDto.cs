using ExaminationSystem.Domain.Entities.Shared.Enums.User;

namespace ExaminationSystem.Features.Authentication.Register.Dto;

public class RegisterDto
{
    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public UserType UserType { get; set; } = UserType.Student;
}