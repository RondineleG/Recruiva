using Microsoft.EntityFrameworkCore;

using Recruiva.Core.Entities;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Web.Repositories;

public class AdvertiserRepository : IBaseRepository<Advertiser>
{
    private readonly ApplicationDbContext _context;

    public AdvertiserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RequestResult<Advertiser>> CreateAsync(Advertiser entity)
    {
        _context.Advertisers.Add(entity);
        await _context.SaveChangesAsync();
        return RequestResult<Advertiser>.Success(entity);
    }

    public async Task<RequestResult<Advertiser>> DeleteAsync(Id id)
    {
        var advertiser = await _context.Advertisers.FindAsync(id.Value);
        if (advertiser == null)
            return RequestResult<Advertiser>.EntityNotFound("Advertiser", id.Value, "Anunciante não encontrado.");

        advertiser.IsDeleted = true;
        advertiser.DeletedAt = DateTime.UtcNow;
        
        _context.Advertisers.Update(advertiser);
        await _context.SaveChangesAsync();
        return RequestResult<Advertiser>.Success(advertiser);
    }

    public async Task<RequestResult<IEnumerable<Advertiser>>> GetAllAsync(int page, int size)
    {
        var advertisers = await _context.Advertisers
            .Include(a => a.Address)
            .Where(a => !a.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return RequestResult<IEnumerable<Advertiser>>.Success(advertisers);
    }

    public async Task<RequestResult<Advertiser>> GetByIdAsync(Id id)
    {
        var advertiser = await _context.Advertisers
            .Include(a => a.Address)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

        if (advertiser == null)
            return RequestResult<Advertiser>.EntityNotFound("Advertiser", id.Value, "Anunciante não encontrado.");

        return RequestResult<Advertiser>.Success(advertiser);
    }

    public async Task<RequestResult<Advertiser>> UpdateAsync(Advertiser entity)
    {
        _context.Advertisers.Update(entity);
        await _context.SaveChangesAsync();
        return RequestResult<Advertiser>.Success(entity);
    }
}
