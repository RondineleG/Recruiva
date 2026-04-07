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
/// UseCase para enviar email ao anunciante quando uma candidatura é criada.
/// </summary>
public sealed class NotifyOnApplicationCreatedUseCase : IUseCase<NotifyOnApplicationCreatedRequest, bool>
{
    private readonly IBaseRepository<Application> _applicationRepository;
    private readonly IBaseRepository<Job> _jobRepository;
    private readonly IBaseRepository<Advertiser> _advertiserRepository;
    private readonly SendEmailNotificationUseCase _sendEmailUseCase;
    private readonly ILogger<NotifyOnApplicationCreatedUseCase> _logger;

    public NotifyOnApplicationCreatedUseCase(
        IBaseRepository<Application> applicationRepository,
        IBaseRepository<Job> jobRepository,
        IBaseRepository<Advertiser> advertiserRepository,
        SendEmailNotificationUseCase sendEmailUseCase,
        ILogger<NotifyOnApplicationCreatedUseCase> logger)
    {
        _applicationRepository = applicationRepository;
        _jobRepository = jobRepository;
        _advertiserRepository = advertiserRepository;
        _sendEmailUseCase = sendEmailUseCase;
        _logger = logger;
    }

    public async Task<RequestResult<bool>> ExecuteAsync(NotifyOnApplicationCreatedRequest request)
    {
        try
        {
            // Buscar candidatura
            var applicationResult = await _applicationRepository.GetByIdAsync(Id.Create(request.ApplicationId));
            if (applicationResult.Status != EResultStatus.Success)
            {
                _logger.LogWarning("Candidatura não encontrada: {ApplicationId}", request.ApplicationId);
                return RequestResult<bool>.WithError("Candidatura não encontrada.");
            }

            var application = applicationResult.Data!;

            // Buscar vaga para obter dados do anunciante
            var jobResult = await _jobRepository.GetByIdAsync(application.JobId);
            if (jobResult.Status != EResultStatus.Success)
            {
                _logger.LogWarning("Vaga não encontrada: {JobId}", application.JobId.Value);
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
            var (subject, html) = EmailTemplates.ApplicationReceivedEmail(advertiser.Name, job.Title);

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
                _logger.LogError("Falha ao enviar email de candidatura para {AdvertiserEmail}: {Error}",
                    advertiser.Email, emailResult.Message);
                return RequestResult<bool>.WithError(emailResult.Message);
            }

            _logger.LogInformation("Email de candidatura enviado com sucesso para {AdvertiserEmail} - Vaga: {JobTitle}",
                advertiser.Email, job.Title);

            return RequestResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao enviar notificação de candidatura criada: {ApplicationId}",
                request.ApplicationId);
            return RequestResult<bool>.WithError($"Erro ao enviar notificação: {ex.Message}");
        }
    }
}

public record NotifyOnApplicationCreatedRequest(Guid ApplicationId);
