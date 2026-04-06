using Recruiva.Core.DTOs.Request;
using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;

namespace Recruiva.Core.UseCases.Jobs;

public class SearchJobsUseCase : IUseCase<ListJobsRequest, ListJobsResponse>
{
    private readonly IBaseRepository<Job> _jobRepository;

    public SearchJobsUseCase(IBaseRepository<Job> jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<RequestResult<ListJobsResponse>> ExecuteAsync(ListJobsRequest request)
    {
        var result = await _jobRepository.GetAllAsync(request.Page, request.Size);
        
        if (result.Status != EResultStatus.Success)
            return RequestResult<ListJobsResponse>.WithError(result.Message);

        var jobs = result.Data!
            .Where(j => FilterJob(j, request))
            .Select(MapToResponse)
            .ToList();

        var response = new ListJobsResponse
        {
            Jobs = jobs,
            TotalCount = jobs.Count,
            Page = request.Page,
            Size = request.Size
        };

        return RequestResult<ListJobsResponse>.Success(response);
    }

    private static bool FilterJob(Job job, ListJobsRequest request)
    {
        // Busca textual
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            if (!job.Title.ToLower().Contains(searchTerm) &&
                !job.Description.ToLower().Contains(searchTerm) &&
                (job.Category == null || !job.Category.ToLower().Contains(searchTerm)))
            {
                return false;
            }
        }

        // Filtro por localização
        if (!string.IsNullOrWhiteSpace(request.City))
        {
            var city = request.City.ToLower();
            if (job.Location == null || job.Location.City == null || !job.Location.City.ToLower().Contains(city))
                return false;
        }

        if (!string.IsNullOrWhiteSpace(request.State))
        {
            var state = request.State.ToLower();
            if (job.Location == null || job.Location.State == null || !job.Location.State.ToLower().Contains(state))
                return false;
        }

        if (request.IsRemote.HasValue)
        {
            if (job.Location == null || job.Location.IsRemote != request.IsRemote.Value)
                return false;
        }

        // Filtro por salário
        if (request.SalaryMin.HasValue)
        {
            if (job.Salary == null || !job.Salary.Max.HasValue || job.Salary.Max < request.SalaryMin.Value)
                return false;
        }

        if (request.SalaryMax.HasValue)
        {
            if (job.Salary == null || !job.Salary.Min.HasValue || job.Salary.Min > request.SalaryMax.Value)
                return false;
        }

        // Filtro por categoria
        if (!string.IsNullOrWhiteSpace(request.Category))
        {
            if (job.Category != request.Category)
                return false;
        }

        return true;
    }

    private static JobResponse MapToResponse(Job job)
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
}
