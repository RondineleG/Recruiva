using Microsoft.Extensions.Logging;

using Recruiva.Core.DTOs.Request;
using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.Services;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.Services;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Core.UseCases.Notifications;

/// <summary>
/// UseCase para enviar email ao anunciante quando uma vaga expira.
/// </summary>
public sealed class NotifyOnJobExpiredUseCase : IUseCase<NotifyOnJobExpiredRequest, bool>
{
    private readonly IBaseRepository<Job> _jobRepository;
    private readonly IBaseRepository<Advertiser> _advertiserRepository;
    private readonly SendEmailNotificationUseCase _sendEmailUseCase;
    private readonly ILogger<NotifyOnJobExpiredUseCase> _logger;

    public NotifyOnJobExpiredUseCase(
        IBaseRepository<Job> jobRepository,
        IBaseRepository<Advertiser> advertiserRepository,
        SendEmailNotificationUseCase sendEmailUseCase,
        ILogger<NotifyOnJobExpiredUseCase> logger)
    {
        _jobRepository = jobRepository;
        _advertiserRepository = advertiserRepository;
        _sendEmailUseCase = sendEmailUseCase;
        _logger = logger;
    }

    public async Task<RequestResult<bool>> ExecuteAsync(NotifyOnJobExpiredRequest request)
    {
        try
        {
            // Buscar vaga
            var jobResult = await _jobRepository.GetByIdAsync(Id.Create(request.JobId));
            if (jobResult.Status != EResultStatus.Success)
            {
                _logger.LogWarning("Vaga não encontrada: {JobId}", request.JobId);
                return RequestResult<bool>.WithError("Vaga não encontrada.");
            }

            var job = jobResult.Data!;

            // Buscar anunciante para obter email
            var advertiserResult = await _advertiserRepository.GetByIdAsync(job.AdvertiserId);
            if (advertiserResult.Status != EResultStatus.Success)
            {
                _logger.LogWarning("Anunciante não encontrado: {AdvertiserId}", job.AdvertiserId.Value);
                return RequestResult<bool>.WithError("Anunciante não encontrado.");
            }

            var advertiser = advertiserResult.Data!;

            // Preparar template de email
            var (subject, html) = EmailTemplates.JobExpiredEmail(advertiser.Name, job.Title);

            // Enviar email
            var emailRequest = new EmailNotificationRequest
            {
                ToEmail = advertiser.Email,
                Subject = subject,
                HtmlContent = html
            };

            var emailResult = await _sendEmailUseCase.ExecuteAsync(emailRequest);

            if (emailResult.Status != EResultStatus.Success)
            {
                _logger.LogError("Falha ao enviar email de expiração para {AdvertiserEmail}: {Error}",
                    advertiser.Email, emailResult.Message);
                return RequestResult<bool>.WithError(emailResult.Message);
            }

            _logger.LogInformation("Email de expiração enviado com sucesso para {AdvertiserEmail} - Vaga: {JobTitle}",
                advertiser.Email, job.Title);

            return RequestResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao enviar notificação de vaga expirada: {JobId}",
                request.JobId);
            return RequestResult<bool>.WithError($"Erro ao enviar notificação: {ex.Message}");
        }
    }
}

public record NotifyOnJobExpiredRequest(Guid JobId);
