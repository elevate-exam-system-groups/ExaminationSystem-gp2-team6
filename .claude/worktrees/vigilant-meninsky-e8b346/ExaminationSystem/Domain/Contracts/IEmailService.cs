namespace ExaminationSystem.Domain.Contracts;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body);
}