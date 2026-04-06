using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Web.Data;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        if (context.Advertisers.Any())
            return;

        var tenantId = "default";

        var tenantConfig = new TenantConfig
        {
            Id = Id.Create(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            TenantId = tenantId,
            DisplayName = "Recruiva",
            BaseUrl = "https://recruiva.com",
            IsActive = true,
            PrimaryThemeColor = "#0d6efd"
        };
        context.TenantConfigs.Add(tenantConfig);

        var addressSP = new Address
        {
            Id = Id.Create(Guid.NewGuid()),
            TenantId = tenantId,
            City = "São Paulo",
            State = "SP",
            Country = "BR",
            District = "Centro",
            Street = "Av. Paulista",
            Number = "1000",
            ZipCode = "01310-100"
        };

        var addressRJ = new Address
        {
            Id = Id.Create(Guid.NewGuid()),
            TenantId = tenantId,
            City = "Rio de Janeiro",
            State = "RJ",
            Country = "BR",
            District = "Centro",
            Street = "Av. Rio Branco",
            Number = "500",
            ZipCode = "20040-001"
        };

        var addressRemote = new Address
        {
            Id = Id.Create(Guid.NewGuid()),
            TenantId = tenantId,
            City = "Remoto",
            State = "SP",
            Country = "BR"
        };

        context.Addresses.AddRange(addressSP, addressRJ, addressRemote);
        await context.SaveChangesAsync();

        var advertiser1 = new Advertiser
        {
            Id = Id.Create(Guid.Parse("22222222-2222-2222-2222-222222222222")),
            TenantId = tenantId,
            Name = "Tech Solutions Ltda",
            Email = "contato@techsolutions.com",
            Phone = "(11) 99999-9999",
            TaxId = "12.345.678/0001-90",
            PersonType = EPersonType.Company,
            AddressId = addressSP.Id,
            Status = EAdvertiserStatus.Active,
            IsEmailVerified = true,
            CompanyDescription = "Empresa especializada em soluções de tecnologia",
            LogoUrl = "https://via.placeholder.com/150",
            Website = "https://techsolutions.com",
            ActivePlan = "Free"
        };

        var advertiser2 = new Advertiser
        {
            Id = Id.Create(Guid.Parse("33333333-3333-3333-3333-333333333333")),
            TenantId = tenantId,
            Name = "Marketing Digital SA",
            Email = "rh@marketingdigital.com",
            Phone = "(21) 98888-8888",
            TaxId = "98.765.432/0001-10",
            PersonType = EPersonType.Company,
            AddressId = addressRJ.Id,
            Status = EAdvertiserStatus.Active,
            IsEmailVerified = true,
            CompanyDescription = "Agência de marketing digital",
            LogoUrl = "https://via.placeholder.com/150",
            Website = "https://marketingdigital.com",
            ActivePlan = "Free"
        };

        context.Advertisers.AddRange(advertiser1, advertiser2);
        await context.SaveChangesAsync();

        var candidate1 = new Candidate
        {
            Id = Id.Create(Guid.Parse("44444444-4444-4444-4444-444444444444")),
            TenantId = tenantId,
            Name = "João Silva",
            Email = "joao.silva@email.com",
            Phone = "(11) 97777-7777",
            DateOfBirth = new DateTime(1990, 5, 15),
            AddressId = addressSP.Id,
            Status = EAccountStatus.Active,
            IsEmailVerified = true,
            LinkedIn = "https://linkedin.com/in/joaosilva"
        };

        var candidate2 = new Candidate
        {
            Id = Id.Create(Guid.Parse("55555555-5555-5555-5555-555555555555")),
            TenantId = tenantId,
            Name = "Maria Santos",
            Email = "maria.santos@email.com",
            Phone = "(21) 96666-6666",
            DateOfBirth = new DateTime(1992, 8, 20),
            AddressId = addressRJ.Id,
            Status = EAccountStatus.Active,
            IsEmailVerified = true,
            LinkedIn = "https://linkedin.com/in/mariasantos"
        };

        context.Candidates.AddRange(candidate1, candidate2);
        await context.SaveChangesAsync();

        var jobs = new List<Job>
        {
            new()
            {
                Id = Id.Create(Guid.NewGuid()),
                TenantId = tenantId,
                AdvertiserId = advertiser1.Id,
                Title = "Desenvolvedor Full Stack Pleno",
                Description = "Buscamos desenvolvedor full stack experiente.",
                Requirements = ".NET, React, SQL Server",
                Category = "Tecnologia",
                Tags = ".NET, React, C#",
                Status = EJobStatus.Active,
                ExpirationDate = DateTime.UtcNow.AddDays(60),
                Location = new JobLocation { City = "São Paulo", State = "SP", Type = "Hybrid", IsRemote = true },
                Salary = new SalaryRange { Min = 8000, Max = 12000, Display = true },
                Moderation = new ModerationInfo { Status = EModerationStatus.Approved },
                Boost = new JobBoost { IsActive = false },
                Highlight = new JobHighlight { IsActive = false },
                Counters = new JobCounters { Views = 45, Applications = 12 }
            },
            new()
            {
                Id = Id.Create(Guid.NewGuid()),
                TenantId = tenantId,
                AdvertiserId = advertiser2.Id,
                Title = "Analista de Marketing Digital",
                Description = "Procuramos analista de marketing digital criativo.",
                Requirements = "Google Ads, SEO, Analytics",
                Category = "Marketing",
                Tags = "Marketing, SEO",
                Status = EJobStatus.Active,
                ExpirationDate = DateTime.UtcNow.AddDays(30),
                Location = new JobLocation { City = "Rio de Janeiro", State = "RJ", Type = "Remote", IsRemote = true },
                Salary = new SalaryRange { Min = 5000, Max = 7000, Display = true },
                Moderation = new ModerationInfo { Status = EModerationStatus.Approved },
                Boost = new JobBoost { IsActive = false },
                Highlight = new JobHighlight { IsActive = false },
                Counters = new JobCounters { Views = 92, Applications = 31 }
            }
        };

        context.Jobs.AddRange(jobs);
        await context.SaveChangesAsync();
    }
}
