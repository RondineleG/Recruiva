using Microsoft.Extensions.Logging;

using Recruiva.Core.DTOs.Request;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Services;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.Services;

namespace Recruiva.Core.UseCases.Notifications;

public sealed class EmailConfirmationUseCase : IUseCase<EmailConfirmationRequest, bool>
{
    private readonly SendEmailNotificationUseCase _sendEmailUseCase;
    private readonly ILogger<EmailConfirmationUseCase> _logger;

    public EmailConfirmationUseCase(
        SendEmailNotificationUseCase sendEmailUseCase,
        ILogger<EmailConfirmationUseCase> logger)
    {
        _sendEmailUseCase = sendEmailUseCase;
        _logger = logger;
    }

    public async Task<RequestResult<bool>> ExecuteAsync(EmailConfirmationRequest request)
    {
        try
        {
            _logger.LogInformation("Enviando email de confirmação para {Email}", request.Email);

            var (subject, html) = EmailTemplates.WelcomeEmail(request.UserName, request.ConfirmationLink);

            var emailRequest = new EmailNotificationRequest
            {
                ToEmail = request.Email,
                Subject = subject,
                HtmlContent = html
            };

            var result = await _sendEmailUseCase.ExecuteAsync(emailRequest);

            if (result.Status == EResultStatus.Success)
            {
                _logger.LogInformation("Email de confirmação enviado com sucesso para {Email}", request.Email);
                return RequestResult<bool>.Success(true);
            }

            _logger.LogWarning("Falha ao enviar email de confirmação para {Email}: {Message}", request.Email, result.Message);
            return RequestResult<bool>.WithError(result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar email de confirmação para {Email}", request.Email);
            return RequestResult<bool>.WithError($"Falha ao enviar email de confirmação: {ex.Message}");
        }
    }
}

public class EmailConfirmationRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string ConfirmationLink { get; set; } = string.Empty;
}
