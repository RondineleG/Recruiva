namespace Recruiva.Core.Interfaces.Services;

public interface IEmailSender
{
    Task SendEmailAsync(string toEmail, string subject, string htmlContent);
}
