using Recruiva.Core.DTOs.Request;
using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Entities;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.Storage;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Interfaces.Validations;
using Recruiva.Core.UseCases.Jobs;
using Recruiva.Core.UseCases.Applications;
using Recruiva.Core.UseCases.Candidates;
using Recruiva.Core.UseCases.Advertisers;
using Recruiva.Core.UseCases.Resumes;
using Recruiva.Core.UseCases.Notifications;
using Recruiva.Core.UseCases.Storage;
using Recruiva.Core.Validations;
using Recruiva.Web.Repositories;
using Recruiva.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole<Guid>>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Registrar Repositórios
builder.Services.AddScoped<IBaseRepository<Job>, JobRepository>();
builder.Services.AddScoped<IBaseRepository<Candidate>, CandidateRepository>();
builder.Services.AddScoped<IBaseRepository<Advertiser>, AdvertiserRepository>();
builder.Services.AddScoped<IBaseRepository<Application>, ApplicationRepository>();
builder.Services.AddScoped<IBaseRepository<Resume>, ResumeRepository>();
builder.Services.AddScoped<IBaseRepository<Notification>, NotificationRepository>();
builder.Services.AddScoped<IBaseRepository<TenantConfig>, TenantConfigRepository>();

// Registrar Validadores
builder.Services.AddScoped<IEntityValidator<Job>, JobValidator>();
builder.Services.AddScoped<IEntityValidator<Candidate>, CandidateValidator>();
builder.Services.AddScoped<IEntityValidator<Advertiser>, AdvertiserValidator>();
builder.Services.AddScoped<IEntityValidator<Resume>, ResumeValidator>();

// Registrar Use Cases de Jobs
builder.Services.AddScoped<CreateJobUseCase>();
builder.Services.AddScoped<ListJobsUseCase>();
builder.Services.AddScoped<GetJobByIdUseCase>();
builder.Services.AddScoped<UpdateJobUseCase>();
builder.Services.AddScoped<DeleteJobUseCase>();
builder.Services.AddScoped<SearchJobsUseCase>();

// Registrar Use Cases de Applications
builder.Services.AddScoped<CreateApplicationUseCase>();
builder.Services.AddScoped<ListApplicationsByJobUseCase>();
builder.Services.AddScoped<ListApplicationsByCandidateUseCase>();
builder.Services.AddScoped<UpdateApplicationStatusUseCase>();

// Registrar Use Cases de Candidates
builder.Services.AddScoped<CreateCandidateUseCase>();

// Registrar Use Cases de Advertisers
builder.Services.AddScoped<CreateAdvertiserUseCase>();

// Registrar Use Cases de Resumes
builder.Services.AddScoped<CreateResumeUseCase>();
builder.Services.AddScoped<UpdateResumeUseCase>();
builder.Services.AddScoped<DeleteResumeUseCase>();
builder.Services.AddScoped<ListResumesByCandidateUseCase>();
builder.Services.AddScoped<GetResumeByIdUseCase>();

// Registrar Validadores
builder.Services.AddScoped<IEntityValidator<Resume>, ResumeValidator>();

// Registrar Use Cases de Notifications
builder.Services.AddScoped<CreateNotificationUseCase>();
builder.Services.AddScoped<ListNotificationsByUserUseCase>();
builder.Services.AddScoped<MarkNotificationAsReadUseCase>();

// Registrar Storage Provider
builder.Services.AddScoped<IStorageProvider, LocalStorageProvider>();

// Registrar Use Case de Upload
builder.Services.AddScoped<UploadFileUseCase>();

// Registrar CurrentUserHelper
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserHelper, CurrentUserHelper>();

// Registrar UseCase de Analytics
builder.Services.AddScoped<Recruiva.Web.UseCases.Analytics.GetDashboardAnalyticsUseCase>();

// Serviços existentes
builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<AddressService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Recruiva.Web.Client._Imports).Assembly);

app.MapAdditionalIdentityEndpoints();

// Seed Data em desenvolvimento
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await SeedData.InitializeAsync(context);
}

app.Run();