using Microsoft.EntityFrameworkCore;

using Recruiva.Core.Entities;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Web.Repositories;

public class ApplicationRepository : IBaseRepository<Application>
{
    private readonly ApplicationDbContext _context;

    public ApplicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RequestResult<Application>> CreateAsync(Application entity)
    {
        var alreadyApplied = await _context.Applications
            .AsNoTracking()
            .AnyAsync(application => !application.IsDeleted &&
                application.CandidateId == entity.CandidateId &&
                application.JobId == entity.JobId);
        if (alreadyApplied)
            return RequestResult<Application>.WithError("Você já se candidatou a esta vaga.");

        try
        {
            _context.Applications.Add(entity);
            await _context.SaveChangesAsync();
            return RequestResult<Application>.Success(entity);
        }
        catch (DbUpdateException)
        {
            _context.Entry(entity).State = EntityState.Detached;
            return RequestResult<Application>.WithError("Não foi possível concluir sua candidatura. Você pode já ter se candidatado a esta vaga.");
        }
    }

    public async Task<RequestResult<Application>> DeleteAsync(Id id)
    {
        var application = await _context.Applications.FindAsync(id);
        if (application == null)
            return RequestResult<Application>.EntityNotFound("Application", id.Value, "Candidatura não encontrada.");

        application.IsDeleted = true;
        application.DeletedAt = DateTime.UtcNow;
        
        _context.Applications.Update(application);
        await _context.SaveChangesAsync();
        return RequestResult<Application>.Success(application);
    }

    public async Task<RequestResult<IEnumerable<Application>>> GetAllAsync(int page, int size)
    {
        var applications = await _context.Applications
            .Include(a => a.Candidate)
            .Include(a => a.Job)
            .Where(a => !a.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return RequestResult<IEnumerable<Application>>.Success(applications);
    }

    public async Task<RequestResult<Application>> GetByIdAsync(Id id)
    {
        var application = await _context.Applications
            .Include(a => a.Candidate)
            .Include(a => a.Job)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

        if (application == null)
            return RequestResult<Application>.EntityNotFound("Application", id.Value, "Candidatura não encontrada.");

        return RequestResult<Application>.Success(application);
    }

    public async Task<RequestResult<Application>> UpdateAsync(Application entity)
    {
        _context.Applications.Update(entity);
        await _context.SaveChangesAsync();
        return RequestResult<Application>.Success(entity);
    }
}
