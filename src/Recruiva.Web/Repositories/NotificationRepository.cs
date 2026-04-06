using Microsoft.EntityFrameworkCore;

using Recruiva.Core.Entities;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Web.Repositories;

public class NotificationRepository : IBaseRepository<Notification>
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RequestResult<Notification>> CreateAsync(Notification entity)
    {
        _context.Notifications.Add(entity);
        await _context.SaveChangesAsync();
        return RequestResult<Notification>.Success(entity);
    }

    public async Task<RequestResult<Notification>> DeleteAsync(Id id)
    {
        var notification = await _context.Notifications.FindAsync(id.Value);
        if (notification == null)
            return RequestResult<Notification>.EntityNotFound("Notification", id.Value, "Notificação não encontrada.");

        notification.IsDeleted = true;
        notification.DeletedAt = DateTime.UtcNow;
        
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
        return RequestResult<Notification>.Success(notification);
    }

    public async Task<RequestResult<IEnumerable<Notification>>> GetAllAsync(int page, int size)
    {
        var notifications = await _context.Notifications
            .Where(n => !n.IsDeleted)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return RequestResult<IEnumerable<Notification>>.Success(notifications);
    }

    public async Task<RequestResult<Notification>> GetByIdAsync(Id id)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted);

        if (notification == null)
            return RequestResult<Notification>.EntityNotFound("Notification", id.Value, "Notificação não encontrada.");

        return RequestResult<Notification>.Success(notification);
    }

    public async Task<RequestResult<Notification>> UpdateAsync(Notification entity)
    {
        _context.Notifications.Update(entity);
        await _context.SaveChangesAsync();
        return RequestResult<Notification>.Success(entity);
    }
}
