using Microsoft.EntityFrameworkCore;

using Recruiva.Core.Entities;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Web.Repositories;

public class TenantConfigRepository : IBaseRepository<TenantConfig>
{
    private readonly ApplicationDbContext _context;

    public TenantConfigRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RequestResult<TenantConfig>> CreateAsync(TenantConfig entity)
    {
        _context.TenantConfigs.Add(entity);
        await _context.SaveChangesAsync();
        return RequestResult<TenantConfig>.Success(entity);
    }

    public async Task<RequestResult<TenantConfig>> DeleteAsync(Id id)
    {
        var tenantConfig = await _context.TenantConfigs.FindAsync(id.Value);
        if (tenantConfig == null)
            return RequestResult<TenantConfig>.EntityNotFound("TenantConfig", id.Value, "Configuração de tenant não encontrada.");

        tenantConfig.IsDeleted = true;
        tenantConfig.DeletedAt = DateTime.UtcNow;
        
        _context.TenantConfigs.Update(tenantConfig);
        await _context.SaveChangesAsync();
        return RequestResult<TenantConfig>.Success(tenantConfig);
    }

    public async Task<RequestResult<IEnumerable<TenantConfig>>> GetAllAsync(int page, int size)
    {
        var tenantConfigs = await _context.TenantConfigs
            .Where(t => !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return RequestResult<IEnumerable<TenantConfig>>.Success(tenantConfigs);
    }

    public async Task<RequestResult<TenantConfig>> GetByIdAsync(Id id)
    {
        var tenantConfig = await _context.TenantConfigs
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        if (tenantConfig == null)
            return RequestResult<TenantConfig>.EntityNotFound("TenantConfig", id.Value, "Configuração de tenant não encontrada.");

        return RequestResult<TenantConfig>.Success(tenantConfig);
    }

    public async Task<RequestResult<TenantConfig>> UpdateAsync(TenantConfig entity)
    {
        _context.TenantConfigs.Update(entity);
        await _context.SaveChangesAsync();
        return RequestResult<TenantConfig>.Success(entity);
    }
}
