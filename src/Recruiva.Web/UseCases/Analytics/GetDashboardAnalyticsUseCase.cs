using Microsoft.EntityFrameworkCore;

using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Requests;

namespace Recruiva.Web.UseCases.Analytics;

public class GetDashboardAnalyticsUseCase
{
    private readonly ApplicationDbContext _context;

    public GetDashboardAnalyticsUseCase(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RequestResult<DashboardAnalyticsResponse>> ExecuteAsync(Guid? advertiserId = null)
    {
        var response = new DashboardAnalyticsResponse();

        // Total de candidatos
        response.TotalCandidates = await _context.Candidates
            .CountAsync(c => !c.IsDeleted);

        // Total de anunciantes
        response.TotalAdvertisers = await _context.Advertisers
            .CountAsync(a => !a.IsDeleted);

        // Total de vagas e vagas ativas
        var allJobs = await _context.Jobs
            .Include(j => j.Advertiser)
            .Where(j => !j.IsDeleted)
            .ToListAsync();

        response.TotalJobs = allJobs.Count;
        response.ActiveJobs = allJobs.Count(j => j.Status == EJobStatus.Active);

        // Vagas por status
        response.JobsByStatus = allJobs
            .GroupBy(j => j.Status.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        // Total de candidaturas
        var allApplications = await _context.Applications
            .Include(a => a.Candidate)
            .Include(a => a.Job)
            .Where(a => !a.IsDeleted)
            .ToListAsync();

        response.TotalApplications = allApplications.Count;

        // Candidaturas por status
        response.ApplicationsByStatus = allApplications
            .GroupBy(a => a.Status.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        // Se advertiserId fornecido, filtrar dados do anunciante
        if (advertiserId.HasValue)
        {
            var advertiserJobs = allJobs.Where(j => j.AdvertiserId.Value == advertiserId.Value).ToList();
            response.TotalJobs = advertiserJobs.Count;
            response.ActiveJobs = advertiserJobs.Count(j => j.Status == EJobStatus.Active);

            response.JobsByStatus = advertiserJobs
                .GroupBy(j => j.Status.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            var advertiserJobIds = advertiserJobs.Select(j => j.Id.Value).ToHashSet();
            var advertiserApplications = allApplications.Where(a => advertiserJobIds.Contains(a.JobId.Value)).ToList();
            response.TotalApplications = advertiserApplications.Count;

            response.ApplicationsByStatus = advertiserApplications
                .GroupBy(a => a.Status.ToString())
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // Vagas recentes (últimas 6)
        response.RecentJobs = allJobs
            .OrderByDescending(j => j.CreatedAt)
            .Take(6)
            .Select(MapToJobResponse)
            .ToList();

        // Candidaturas recentes (últimas 6)
        response.RecentApplications = allApplications
            .OrderByDescending(a => a.AppliedAt ?? a.CreatedAt)
            .Take(6)
            .Select(MapToApplicationResponse)
            .ToList();

        return RequestResult<DashboardAnalyticsResponse>.Success(response);
    }

    private static JobResponse MapToJobResponse(Job job)
    {
        return new JobResponse
        {
            Id = job.Id.Value,
            Title = job.Title,
            Description = job.Description,
            Requirements = job.Requirements,
            Responsibilities = job.Responsibilities,
            Benefits = job.Benefits,
            Category = job.Category,
            Tags = job.Tags,
            ExpirationDate = job.ExpirationDate,
            Status = job.Status.ToString(),
            City = job.Location?.City,
            State = job.Location?.State,
            Country = job.Location?.Country ?? "BR",
            Type = job.Location?.Type ?? "OnSite",
            IsRemote = job.Location?.IsRemote ?? false,
            SalaryMin = job.Salary?.Min,
            SalaryMax = job.Salary?.Max,
            SalaryCurrency = job.Salary?.Currency ?? "BRL",
            SalaryDisplay = job.Salary?.Display ?? true,
            Views = job.Counters?.Views ?? 0,
            Applications = job.Counters?.Applications ?? 0,
            AdvertiserId = job.AdvertiserId.Value,
            AdvertiserName = job.Advertiser?.Name,
            AdvertiserLogoUrl = job.Advertiser?.LogoUrl,
            CreatedAt = job.CreatedAt,
            UpdatedAt = job.UpdatedAt
        };
    }

    private static ApplicationResponse MapToApplicationResponse(Application application)
    {
        return new ApplicationResponse
        {
            Id = application.Id.Value,
            CandidateId = application.CandidateId.Value,
            CandidateName = application.Candidate?.Name,
            CandidateEmail = application.Candidate?.Email,
            JobId = application.JobId.Value,
            JobTitle = application.Job?.Title,
            Status = application.Status.ToString(),
            AppliedAt = application.AppliedAt,
            ViewedAt = application.ViewedAt,
            SelectedAt = application.SelectedAt,
            RejectedAt = application.RejectedAt,
            Notes = application.Notes,
            CreatedAt = application.CreatedAt
        };
    }
}
