using Microsoft.EntityFrameworkCore;

using Recruiva.Core.Entities;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Web.Repositories;

public class JobRepository : IBaseRepository<Job>
{
    private readonly ApplicationDbContext _context;

    public JobRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RequestResult<Job>> CreateAsync(Job entity)
    {
        _context.Jobs.Add(entity);
        await _context.SaveChangesAsync();
        return RequestResult<Job>.Success(entity);
    }

    public async Task<RequestResult<Job>> DeleteAsync(Id id)
    {
        var job = await _context.Jobs.FindAsync(id.Value);
        if (job == null)
            return RequestResult<Job>.EntityNotFound("Job", id.Value, "Vaga não encontrada.");

        job.IsDeleted = true;
        job.DeletedAt = DateTime.UtcNow;
        
        _context.Jobs.Update(job);
        await _context.SaveChangesAsync();
        return RequestResult<Job>.Success(job);
    }

    public async Task<RequestResult<IEnumerable<Job>>> GetAllAsync(int page, int size)
    {
        var jobs = await _context.Jobs
            .Include(j => j.Advertiser)
            .Where(j => !j.IsDeleted && j.Status == EJobStatus.Active)
            .OrderByDescending(j => j.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return RequestResult<IEnumerable<Job>>.Success(jobs);
    }

    public async Task<RequestResult<Job>> GetByIdAsync(Id id)
    {
        var job = await _context.Jobs
            .Include(j => j.Advertiser)
            .FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted);

        if (job == null)
            return RequestResult<Job>.EntityNotFound("Job", id.Value, "Vaga não encontrada.");

        return RequestResult<Job>.Success(job);
    }

    public async Task<RequestResult<Job>> UpdateAsync(Job entity)
    {
        _context.Jobs.Update(entity);
        await _context.SaveChangesAsync();
        return RequestResult<Job>.Success(entity);
    }
}
