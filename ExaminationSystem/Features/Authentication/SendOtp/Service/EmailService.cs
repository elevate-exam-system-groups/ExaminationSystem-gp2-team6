using System.Net;
using System.Net.Mail;
using ExaminationSystem.Contracts.Email;
using ExaminationSystem.Domain.Entities.Email;
using Microsoft.Extensions.Options;

namespace ExaminationSystem.Features.Authentication.SendOtp.Service;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(_settings.Host, _settings.Port);
        client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
        client.EnableSsl = true;

        var mail = new MailMessage
        {
            From = new MailAddress(_settings.From, _settings.DisplayName),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };

        mail.To.Add(to);

        await client.SendMailAsync(mail);
    }
}