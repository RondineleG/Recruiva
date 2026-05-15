using Microsoft.EntityFrameworkCore;

using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Entities;
using Recruiva.Core.Interfaces.Repositories;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Web.Repositories;

public class JobRepository : IJobRepository
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

    public async Task<PagedResponse<Job>> GetAllPagedAsync(int page, int size)
    {
        var query = _context.Jobs
            .Include(j => j.Advertiser)
            .Where(j => !j.IsDeleted && j.Status == EJobStatus.Active);

        var totalCount = await query.CountAsync();
        var jobs = await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new PagedResponse<Job>
        {
            Data = jobs,
            TotalCount = totalCount,
            Page = page,
            Size = size
        };
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

    public async Task<RequestResult<IEnumerable<Job>>> SearchAsync(
        string? searchTerm = null,
        string? city = null,
        string? state = null,
        bool? isRemote = null,
        decimal? salaryMin = null,
        decimal? salaryMax = null,
        string? category = null,
        Guid? advertiserId = null,
        int page = 1,
        int size = 10)
    {
        var query = _context.Jobs
            .Include(j => j.Advertiser)
            .Include(j => j.Location)
            .Include(j => j.Salary)
            .Where(j => !j.IsDeleted && j.Status == EJobStatus.Active);

        // Filtro por advertiserId (para MyJobs)
        if (advertiserId.HasValue)
        {
            query = query.Where(j => j.AdvertiserId.Value == advertiserId.Value);
        }

        // Filtro por busca textual (aplicado no WHERE do SQL)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(j =>
                EF.Functions.Like(j.Title, $"%{term}%") ||
                EF.Functions.Like(j.Description, $"%{term}%") ||
                (j.Category != null && EF.Functions.Like(j.Category, $"%{term}%")));
        }

        // Filtro por cidade
        if (!string.IsNullOrWhiteSpace(city))
        {
            var cityLower = city.ToLower();
            query = query.Where(j => j.Location != null && EF.Functions.Like(j.Location.City, $"%{cityLower}%"));
        }

        // Filtro por estado
        if (!string.IsNullOrWhiteSpace(state))
        {
            var stateUpper = state.ToUpper();
            query = query.Where(j => j.Location != null && j.Location.State == stateUpper);
        }

        // Filtro por remoto
        if (isRemote.HasValue)
        {
            query = query.Where(j => j.Location != null && j.Location.IsRemote == isRemote.Value);
        }

        // Filtro por salário mínimo
        if (salaryMin.HasValue)
        {
            query = query.Where(j => j.Salary != null && j.Salary.Max.HasValue && j.Salary.Max >= salaryMin.Value);
        }

        // Filtro por salário máximo
        if (salaryMax.HasValue)
        {
            query = query.Where(j => j.Salary != null && j.Salary.Min.HasValue && j.Salary.Min <= salaryMax.Value);
        }

        // Filtro por categoria
        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(j => j.Category == category);
        }

        // Paginação com OFFSET/FETCH
        var totalJobs = await query.CountAsync();
        var jobs = await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return RequestResult<IEnumerable<Job>>.Success(jobs);
    }
}
