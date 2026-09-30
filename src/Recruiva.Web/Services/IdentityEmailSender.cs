namespace Recruiva.Web.Services;

public sealed class IdentityEmailSender(
    Recruiva.Core.Interfaces.Services.IEmailSender emailSender)
    : Microsoft.AspNetCore.Identity.IEmailSender<ApplicationUser>
{
    public Task SendConfirmationLinkAsync(
        ApplicationUser user,
        string email,
        string confirmationLink) =>
        emailSender.SendEmailAsync(
            email,
            "Confirme sua conta Recruiva",
            $"<p>Olá!</p><p>Confirme sua conta acessando <a href=\"{confirmationLink}\">este link</a>.</p>");

    public Task SendPasswordResetLinkAsync(
        ApplicationUser user,
        string email,
        string resetLink) =>
        emailSender.SendEmailAsync(
            email,
            "Redefinição de senha Recruiva",
            $"<p>Redefina sua senha acessando <a href=\"{resetLink}\">este link</a>.</p>");

    public Task SendPasswordResetCodeAsync(
        ApplicationUser user,
        string email,
        string resetCode) =>
        emailSender.SendEmailAsync(
            email,
            "Código para redefinição de senha Recruiva",
            $"<p>Seu código para redefinir a senha é: <strong>{resetCode}</strong></p>");
}
