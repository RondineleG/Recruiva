using Recruiva.Core.DTOs.Request;
using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Core.UseCases.Notifications;

public class CreateNotificationUseCase : IUseCase<CreateNotificationRequest, NotificationResponse>
{
    private readonly IBaseRepository<Notification> _notificationRepository;

    public CreateNotificationUseCase(IBaseRepository<Notification> notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<RequestResult<NotificationResponse>> ExecuteAsync(CreateNotificationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return RequestResult<NotificationResponse>.WithValidationError("Título é obrigatório.", "Title");

        if (string.IsNullOrWhiteSpace(request.Message))
            return RequestResult<NotificationResponse>.WithValidationError("Mensagem é obrigatória.", "Message");

        if (string.IsNullOrWhiteSpace(request.RecipientId))
            return RequestResult<NotificationResponse>.WithValidationError("Destinatário é obrigatório.", "RecipientId");

        var notification = new Notification
        {
            Id = Id.Create(Guid.NewGuid()),
            Title = request.Title,
            Message = request.Message,
            RecipientId = request.RecipientId,
            Type = request.Type,
            IsRead = false,
            TenantId = "default"
        };

        var result = await _notificationRepository.CreateAsync(notification);
        if (result.Status != EResultStatus.Success)
            return RequestResult<NotificationResponse>.WithError(result.Message);

        return RequestResult<NotificationResponse>.Success(MapToResponse(result.Data!));
    }

    private static NotificationResponse MapToResponse(Notification notification) => new()
    {
        Id = notification.Id.Value,
        Title = notification.Title ?? string.Empty,
        Message = notification.Message ?? string.Empty,
        RecipientId = notification.RecipientId,
        Type = notification.Type,
        IsRead = notification.IsRead,
        ReadAt = notification.ReadAt,
        CreatedAt = notification.CreatedAt
    };
}
