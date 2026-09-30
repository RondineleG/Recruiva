using Microsoft.Playwright;

namespace Recruiva.E2ETests;

public class PublicPagesTests : E2ETestBase
{
    public PublicPagesTests(PlaywrightFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Home_ShouldLoadDashboard()
    {
        var response = await GotoAsync("/").ConfigureAwait(false);
        Assert.NotNull(response);
        Assert.True(response!.Ok, $"Home retornou {response.Status}");

        await WaitForBlazorAsync().ConfigureAwait(false);
        Assert.Contains("Recruiva", await Page.TitleAsync().ConfigureAwait(false));
    }

    [Fact]
    public async Task AnonymousHome_ShouldShowPublicLandingWithoutRecruiterMetrics()
    {
        await GotoAsync("/").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);

        var body = await Page.Locator("body").InnerTextAsync().ConfigureAwait(false);
        Assert.Contains("Explorar vagas", body);
        Assert.DoesNotContain("SEU ESPAÇO RECRUIVA", body);
        Assert.DoesNotContain("Total de vagas", body);
        Assert.Equal(1, await Page.Locator(".app-sidebar .sidebar-link").CountAsync().ConfigureAwait(false));
        Assert.Equal(0, await Page.GetByTestId("nav-plans").CountAsync().ConfigureAwait(false));
    }

    [Fact]
    public async Task Health_ShouldReturnOk()
    {
        var api = await Fixture.Playwright.APIRequest.NewContextAsync().ConfigureAwait(false);
        var response = await api.GetAsync($"{BaseUrl}/health").ConfigureAwait(false);
        Assert.True(response.Ok);
    }

    [Fact]
    public async Task Jobs_ShouldList_SeededJobs()
    {
        var response = await GotoAsync("/jobs").ConfigureAwait(false);
        Assert.True(response!.Ok, $"Jobs retornou {response.Status}");

        await WaitForBlazorAsync().ConfigureAwait(false);

        var cards = Page.GetByTestId("job-card");
        await cards.First.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible
        }).ConfigureAwait(false);

        Assert.True(await cards.CountAsync().ConfigureAwait(false) > 0,
            "Nenhuma vaga listada — seed deveria criar vagas");
    }

    [Fact]
    public async Task Jobs_Search_ShouldFilterResults()
    {
        await GotoAsync("/jobs").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);

        await Page.GetByTestId("job-search")
            .FillAsync("Desenvolvedor").ConfigureAwait(false);
        await Page.GetByTestId("job-search-submit").ClickAsync().ConfigureAwait(false);
        await Page.WaitForFunctionAsync("""
            () => {
                const cards = Array.from(document.querySelectorAll('[data-testid="job-card"]'));
                return cards.length > 0 && cards.every(card => card.textContent.toLowerCase().includes("desenvolvedor"));
            }
            """).ConfigureAwait(false);

        var cards = Page.GetByTestId("job-card");
        Assert.True(await cards.CountAsync().ConfigureAwait(false) > 0,
            "Busca por 'Desenvolvedor' não retornou vagas");

        var titles = await cards.AllTextContentsAsync().ConfigureAwait(false);
        Assert.All(titles, t => Assert.Contains("Desenvolvedor", t, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Jobs_Search_NoMatch_ShouldShowEmptyState()
    {
        await GotoAsync("/jobs").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);

        await Page.GetByTestId("job-search")
            .FillAsync("xyznonexistentterm").ConfigureAwait(false);
        await Page.GetByTestId("job-search-submit").ClickAsync().ConfigureAwait(false);
        await Page.GetByTestId("jobs-empty").WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible
        }).ConfigureAwait(false);

        Assert.True(await Page.GetByTestId("jobs-empty")
            .IsVisibleAsync().ConfigureAwait(false),
            "Estado vazio não exibido para busca sem resultados");
    }

    [Fact]
    public async Task Jobs_Mobile_ShouldFitViewport_WithoutHorizontalOverflow()
    {
        await Page.SetViewportSizeAsync(390, 844).ConfigureAwait(false);
        await GotoAsync("/jobs").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);

        var layoutFits = await Page.EvaluateAsync<bool>("""
            () => {
                const width = document.documentElement.clientWidth;
                const selectors = [".app-topbar", ".jobs-page", ".jobs-search-panel", ".jobs-grid", ".job-card"];
                return selectors.every(selector => Array.from(document.querySelectorAll(selector)).every(element => {
                    const bounds = element.getBoundingClientRect();
                    return bounds.left >= -1 && bounds.right <= width + 1;
                }));
            }
            """).ConfigureAwait(false);

        Assert.True(layoutFits, "Um dos principais elementos da página ultrapassou a viewport mobile.");
        Assert.True(await Page.GetByTestId("job-search").IsVisibleAsync().ConfigureAwait(false));
    }

    [Fact]
    public async Task MobileNavigation_ShouldOpenAndClose()
    {
        await Page.SetViewportSizeAsync(390, 844).ConfigureAwait(false);
        await GotoAsync("/jobs").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);

        var menuButton = Page.GetByTestId("mobile-menu-toggle");
        Assert.True(await menuButton.IsVisibleAsync().ConfigureAwait(false));
        await menuButton.ClickAsync().ConfigureAwait(false);

        var sidebar = Page.GetByTestId("app-sidebar");
        Assert.Contains("app-sidebar--open", (await sidebar.GetAttributeAsync("class").ConfigureAwait(false)) ?? string.Empty);
        await Page.GetByTestId("nav-jobs").ClickAsync().ConfigureAwait(false);
        Assert.DoesNotContain("app-sidebar--open", (await sidebar.GetAttributeAsync("class").ConfigureAwait(false)) ?? string.Empty);
    }

    [Theory]
    [InlineData("/Account/Login")]
    [InlineData("/Account/Register")]
    public async Task AccountPages_Mobile_ShouldFitViewport(string path)
    {
        await Page.SetViewportSizeAsync(390, 844).ConfigureAwait(false);
        await GotoAsync(path).ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);

        var viewportWidth = await Page.EvaluateAsync<int>("() => document.documentElement.clientWidth").ConfigureAwait(false);
        var documentWidth = await Page.EvaluateAsync<int>("() => document.documentElement.scrollWidth").ConfigureAwait(false);
        Assert.True(documentWidth <= viewportWidth, $"{path} ultrapassou a viewport: {documentWidth}px > {viewportWidth}px");
    }

    [Fact]
    public async Task JobDetails_ShouldOpen_InModal_FromList()
    {
        await GotoAsync("/jobs").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);

        var detailsButton = Page.GetByTestId("job-card-details").First;
        await detailsButton.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible
        }).ConfigureAwait(false);
        await detailsButton.ClickAsync().ConfigureAwait(false);
        await Page.GetByTestId("job-details-modal").WaitForAsync().ConfigureAwait(false);

        Assert.True(await Page.GetByTestId("job-modal-apply")
            .IsVisibleAsync().ConfigureAwait(false),
            "Botão de candidatura não encontrado no modal de detalhes");
    }

    [Fact]
    public async Task Plans_ShouldList_SeededPlans()
    {
        await LoginOrRegisterAsync(AdvertiserEmail, TestPassword).ConfigureAwait(false);
        var response = await GotoAsync("/plans").ConfigureAwait(false);
        Assert.True(response!.Ok, $"Plans retornou {response.Status}");

        await WaitForBlazorAsync().ConfigureAwait(false);
        var content = await Page.ContentAsync().ConfigureAwait(false);
        Assert.Contains("Premium", content);
    }

    [Theory]
    [InlineData("/privacy")]
    [InlineData("/terms")]
    [InlineData("/address/list")]
    public async Task StaticPages_ShouldReturnOk(string path)
    {
        var response = await GotoAsync(path).ConfigureAwait(false);
        Assert.NotNull(response);
        Assert.True(response!.Ok, $"{path} retornou {response.Status}");
    }
}
