using Microsoft.Playwright;

namespace Recruiva.E2ETests;

public class CandidateFlowTests : E2ETestBase
{
    public CandidateFlowTests(PlaywrightFixture fixture) : base(fixture)
    {
    }

    private async Task LoginAsCandidateAsync() =>
        await LoginOrRegisterAsync(CandidateEmail, TestPassword).ConfigureAwait(false);

    [Fact]
    public async Task Candidate_Dashboard_ShouldShowCareerContentOnly()
    {
        await LoginAsCandidateAsync().ConfigureAwait(false);
        await GotoAsync("/dashboard").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);

        var body = await Page.Locator("body").InnerTextAsync().ConfigureAwait(false);
        Assert.Contains("SUA JORNADA", body);
        Assert.DoesNotContain("SEU ESPAÇO RECRUIVA", body);
        Assert.DoesNotContain("Total de vagas", body);
        Assert.Contains("Minhas candidaturas", body);
    }

    [Fact]
    public async Task Candidate_Profile_ShouldLoad_SeededData()
    {
        await LoginAsCandidateAsync().ConfigureAwait(false);

        var response = await GotoAsync("/profile/candidate").ConfigureAwait(false);
        Assert.True(response!.Ok, $"Profile retornou {response.Status}");

        await WaitForBlazorAsync().ConfigureAwait(false);
        var name = await Page.GetByTestId("candidate-profile-name").InputValueAsync().ConfigureAwait(false);
        Assert.Equal("João Silva", name);
    }

    [Fact]
    public async Task Candidate_Navigation_ShouldShowCareerLinksOnly()
    {
        await LoginAsCandidateAsync().ConfigureAwait(false);
        await GotoAsync("/jobs").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);

        Assert.True(await Page.GetByTestId("nav-applications").IsVisibleAsync().ConfigureAwait(false));
        Assert.Equal(0, await Page.GetByTestId("nav-my-jobs").CountAsync().ConfigureAwait(false));
    }

    [Fact]
    public async Task Candidate_MyApplications_ShouldLoad()
    {
        await LoginAsCandidateAsync().ConfigureAwait(false);

        var response = await GotoAsync("/my-applications").ConfigureAwait(false);
        Assert.True(response!.Ok, $"MyApplications retornou {response.Status}");

        await WaitForBlazorAsync().ConfigureAwait(false);
    }

    [Fact]
    public async Task Candidate_Can_ApplyToJob()
    {
        var candidateEmail = $"candidatura-{Guid.NewGuid():N}@test.com";
        await LoginOrRegisterAsync(candidateEmail, TestPassword).ConfigureAwait(false);

        await GotoAsync("/jobs").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);

        var detailsButton = Page.GetByTestId("job-card-details").First;
        await detailsButton.ClickAsync().ConfigureAwait(false);
        await Page.GetByTestId("job-details-modal").WaitForAsync().ConfigureAwait(false);

        await Page.GetByTestId("job-modal-apply").ClickAsync().ConfigureAwait(false);
        try
        {
            await Page.WaitForFunctionAsync(
                "() => location.pathname.startsWith('/apply/')",
                null,
                new PageWaitForFunctionOptions { Timeout = 30000 }).ConfigureAwait(false);
        }
        catch (TimeoutException)
        {
            var modalOpen = await Page.GetByTestId("job-details-modal").CountAsync().ConfigureAwait(false);
            var applyVisible = await Page.GetByTestId("job-modal-apply").IsVisibleAsync().ConfigureAwait(false);
            var ready = await Page.EvaluateAsync<string?>(
                "() => document.querySelector('[data-testid=\"app-shell\"]')?.getAttribute('data-blazor-ready')").ConfigureAwait(false);
            throw new Exception(
                $"Navegação para /apply falhou. url={Page.Url} modalOpen={modalOpen} applyVisible={applyVisible} blazorReady={ready}");
        }
        await WaitForBlazorAsync().ConfigureAwait(false);

        var submit = Page.GetByTestId("application-submit");
        Assert.True(await submit.IsEnabledAsync().ConfigureAwait(false),
            "Botão de candidatura deveria estar habilitado");

        await Page.GetByTestId("application-notes")
            .FillAsync("Candidatura E2E automatizada").ConfigureAwait(false);
        await submit.ClickAsync().ConfigureAwait(false);
        await Page.GetByTestId("application-feedback").WaitForAsync(new LocatorWaitForOptions
        {
            Timeout = 45000
        }).ConfigureAwait(false);

        Assert.True(await Page.GetByTestId("application-feedback").IsVisibleAsync().ConfigureAwait(false),
            "Nenhum feedback exibido após enviar candidatura");
    }

    [Fact]
    public async Task Candidate_DuplicateApplication_ShouldShowFriendlyFeedback()
    {
        await LoginAsCandidateAsync().ConfigureAwait(false);
        await GotoAsync("/jobs").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);

        await Page.GetByTestId("job-search").FillAsync("Desenvolvedor Full Stack Pleno").ConfigureAwait(false);
        await Page.GetByTestId("job-search-submit").ClickAsync().ConfigureAwait(false);
        await Page.WaitForFunctionAsync("() => document.querySelector('.job-card__title')?.textContent?.trim() === 'Desenvolvedor Full Stack Pleno'")
            .ConfigureAwait(false);
        await Page.GetByTestId("job-card-details").First.ClickAsync().ConfigureAwait(false);
        await Page.GetByTestId("job-details-modal").WaitForAsync().ConfigureAwait(false);
        await Page.GetByTestId("job-modal-apply").ClickAsync().ConfigureAwait(false);
        await Page.WaitForFunctionAsync("() => location.pathname.startsWith('/apply/')").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);
        await Page.GetByTestId("application-submit").ClickAsync().ConfigureAwait(false);
        await Page.GetByTestId("application-feedback").WaitForAsync().ConfigureAwait(false);

        Assert.Contains("já se candidatou", await Page.GetByTestId("application-feedback").InnerTextAsync().ConfigureAwait(false), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Candidate_Resumes_ShouldLoad()
    {
        await LoginAsCandidateAsync().ConfigureAwait(false);

        var response = await GotoAsync("/resumes").ConfigureAwait(false);
        Assert.True(response!.Ok, $"Resumes retornou {response.Status}");
        await WaitForBlazorAsync().ConfigureAwait(false);
    }

    [Fact]
    public async Task Candidate_ResumeCreate_ShouldRenderForm()
    {
        await LoginAsCandidateAsync().ConfigureAwait(false);

        var response = await GotoAsync("/resumes/new").ConfigureAwait(false);
        Assert.True(response!.Ok, $"ResumeCreate retornou {response.Status}");

        await WaitForBlazorAsync().ConfigureAwait(false);
        var submit = Page.GetByTestId("resume-create-submit");
        Assert.True(await submit.IsVisibleAsync().ConfigureAwait(false),
            "Formulário de criação de currículo não renderizou");
    }

    [Fact]
    public async Task Candidate_Notifications_ShouldLoad()
    {
        await LoginAsCandidateAsync().ConfigureAwait(false);

        var response = await GotoAsync("/notifications").ConfigureAwait(false);
        Assert.True(response!.Ok, $"Notifications retornou {response.Status}");
        await WaitForBlazorAsync().ConfigureAwait(false);
    }

    [Fact]
    public async Task Candidate_MySubscription_ShouldLoad()
    {
        await LoginAsCandidateAsync().ConfigureAwait(false);

        var response = await GotoAsync("/my-subscription").ConfigureAwait(false);
        Assert.True(response!.Ok, $"MySubscription retornou {response.Status}");
        await WaitForBlazorAsync().ConfigureAwait(false);
    }
}
