using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;

namespace Recruiva.Core.UseCases.Notifications;

public class ListNotificationsByUserUseCase : IUseCase<string, List<NotificationResponse>>
{
    private readonly IBaseRepository<Entities.Notification> _notificationRepository;

    public ListNotificationsByUserUseCase(IBaseRepository<Entities.Notification> notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<RequestResult<List<NotificationResponse>>> ExecuteAsync(string recipientId)
    {
        if (string.IsNullOrWhiteSpace(recipientId))
            return RequestResult<List<NotificationResponse>>.WithValidationError("ID do destinatário é obrigatório.", "RecipientId");

        var result = await _notificationRepository.GetAllAsync(1, 1000);
        if (result.Status != EResultStatus.Success)
            return RequestResult<List<NotificationResponse>>.WithError(result.Message);

        var userNotifications = result.Data!
            .Where(n => n.RecipientId == recipientId)
            .OrderByDescending(n => n.CreatedAt)
            .ToList();

        var response = userNotifications.Select(MapToResponse).ToList();

        return RequestResult<List<NotificationResponse>>.Success(response);
    }

    private static NotificationResponse MapToResponse(Entities.Notification notification) => new()
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
