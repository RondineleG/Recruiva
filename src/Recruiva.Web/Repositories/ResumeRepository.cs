using Microsoft.EntityFrameworkCore;

using Recruiva.Core.Entities;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Web.Repositories;

public class ResumeRepository : IBaseRepository<Resume>
{
    private readonly ApplicationDbContext _context;

    public ResumeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RequestResult<Resume>> CreateAsync(Resume entity)
    {
        _context.Resumes.Add(entity);
        await _context.SaveChangesAsync();
        return RequestResult<Resume>.Success(entity);
    }

    public async Task<RequestResult<Resume>> DeleteAsync(Id id)
    {
        var resume = await _context.Resumes.FindAsync(id.Value);
        if (resume == null)
            return RequestResult<Resume>.EntityNotFound("Resume", id.Value, "Currículo não encontrado.");

        resume.IsDeleted = true;
        resume.DeletedAt = DateTime.UtcNow;
        
        _context.Resumes.Update(resume);
        await _context.SaveChangesAsync();
        return RequestResult<Resume>.Success(resume);
    }

    public async Task<RequestResult<IEnumerable<Resume>>> GetAllAsync(int page, int size)
    {
        var resumes = await _context.Resumes
            .Include(r => r.Candidate)
            .Where(r => !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return RequestResult<IEnumerable<Resume>>.Success(resumes);
    }

    public async Task<RequestResult<Resume>> GetByIdAsync(Id id)
    {
        var resume = await _context.Resumes
            .Include(r => r.Candidate)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

        if (resume == null)
            return RequestResult<Resume>.EntityNotFound("Resume", id.Value, "Currículo não encontrado.");

        return RequestResult<Resume>.Success(resume);
    }

    public async Task<RequestResult<Resume>> UpdateAsync(Resume entity)
    {
        _context.Resumes.Update(entity);
        await _context.SaveChangesAsync();
        return RequestResult<Resume>.Success(entity);
    }
}
