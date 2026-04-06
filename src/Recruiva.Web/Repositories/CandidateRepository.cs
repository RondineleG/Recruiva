using Microsoft.EntityFrameworkCore;

using Recruiva.Core.Entities;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Web.Repositories;

public class CandidateRepository : IBaseRepository<Candidate>
{
    private readonly ApplicationDbContext _context;

    public CandidateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RequestResult<Candidate>> CreateAsync(Candidate entity)
    {
        _context.Candidates.Add(entity);
        await _context.SaveChangesAsync();
        return RequestResult<Candidate>.Success(entity);
    }

    public async Task<RequestResult<Candidate>> DeleteAsync(Id id)
    {
        var candidate = await _context.Candidates.FindAsync(id.Value);
        if (candidate == null)
            return RequestResult<Candidate>.EntityNotFound("Candidate", id.Value, "Candidato não encontrado.");

        candidate.IsDeleted = true;
        candidate.DeletedAt = DateTime.UtcNow;
        
        _context.Candidates.Update(candidate);
        await _context.SaveChangesAsync();
        return RequestResult<Candidate>.Success(candidate);
    }

    public async Task<RequestResult<IEnumerable<Candidate>>> GetAllAsync(int page, int size)
    {
        var candidates = await _context.Candidates
            .Include(c => c.Address)
            .Where(c => !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return RequestResult<IEnumerable<Candidate>>.Success(candidates);
    }

    public async Task<RequestResult<Candidate>> GetByIdAsync(Id id)
    {
        var candidate = await _context.Candidates
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (candidate == null)
            return RequestResult<Candidate>.EntityNotFound("Candidate", id.Value, "Candidato não encontrado.");

        return RequestResult<Candidate>.Success(candidate);
    }

    public async Task<RequestResult<Candidate>> UpdateAsync(Candidate entity)
    {
        _context.Candidates.Update(entity);
        await _context.SaveChangesAsync();
        return RequestResult<Candidate>.Success(entity);
    }
}
