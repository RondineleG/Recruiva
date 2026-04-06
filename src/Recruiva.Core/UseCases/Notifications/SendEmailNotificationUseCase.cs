using Microsoft.Extensions.Logging;

using Recruiva.Core.DTOs.Request;
using Recruiva.Core.Interfaces.Services;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;

namespace Recruiva.Core.UseCases.Notifications;

public sealed class SendEmailNotificationUseCase : IUseCase<EmailNotificationRequest, bool>
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger<SendEmailNotificationUseCase> _logger;

    public SendEmailNotificationUseCase(IEmailSender emailSender, ILogger<SendEmailNotificationUseCase> logger)
    {
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<RequestResult<bool>> ExecuteAsync(EmailNotificationRequest request)
    {
        try
        {
            _logger.LogInformation("Enviando email para {ToEmail} - Assunto: {Subject}", request.ToEmail, request.Subject);

            await _emailSender.SendEmailAsync(request.ToEmail, request.Subject, request.HtmlContent);

            _logger.LogInformation("Email enviado com sucesso para {ToEmail}", request.ToEmail);

            return RequestResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar email para {ToEmail}: {Subject}", request.ToEmail, request.Subject);
            return RequestResult<bool>.WithError($"Falha ao enviar email: {ex.Message}");
        }
    }
}
