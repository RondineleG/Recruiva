using Microsoft.Playwright;

namespace Recruiva.E2ETests;

public class AdvertiserFlowTests : E2ETestBase
{
    public AdvertiserFlowTests(PlaywrightFixture fixture) : base(fixture)
    {
    }

    private async Task LoginAsAdvertiserAsync() =>
        await LoginOrRegisterAsync(AdvertiserEmail, TestPassword).ConfigureAwait(false);

    [Fact]
    public async Task Advertiser_Profile_ShouldLoad_SeededData()
    {
        await LoginAsAdvertiserAsync().ConfigureAwait(false);

        var response = await GotoAsync("/profile/advertiser").ConfigureAwait(false);
        Assert.True(response!.Ok, $"AdvertiserProfile retornou {response.Status}");

        await WaitForBlazorAsync().ConfigureAwait(false);
        var name = await Page.GetByTestId("advertiser-profile-name").InputValueAsync().ConfigureAwait(false);
        Assert.Equal("Tech Solutions Ltda", name);
    }

    [Fact]
    public async Task Advertiser_Navigation_ShouldShowRecruitmentLinksOnly()
    {
        await LoginAsAdvertiserAsync().ConfigureAwait(false);
        await GotoAsync("/jobs").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);

        Assert.True(await Page.GetByTestId("nav-my-jobs").IsVisibleAsync().ConfigureAwait(false));
        Assert.True(await Page.GetByTestId("nav-received-applications").IsVisibleAsync().ConfigureAwait(false));
        Assert.Equal(0, await Page.GetByTestId("nav-applications").CountAsync().ConfigureAwait(false));
    }

    [Fact]
    public async Task Advertiser_MyJobs_ShouldList_SeededJobs()
    {
        await LoginAsAdvertiserAsync().ConfigureAwait(false);

        var response = await GotoAsync("/my-jobs").ConfigureAwait(false);
        Assert.True(response!.Ok, $"MyJobs retornou {response.Status}");

        await WaitForBlazorAsync().ConfigureAwait(false);
        var jobRows = Page.GetByTestId("my-job-row");
        Assert.True(await jobRows.CountAsync().ConfigureAwait(false) > 0,
            "Nenhuma vaga listada para o anunciante seedado");
    }

    [Fact]
    public async Task Advertiser_Can_CreateJob()
    {
        await LoginAsAdvertiserAsync().ConfigureAwait(false);

        await GotoAsync("/jobs/create").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);

        var jobTitle = $"Vaga E2E {DateTime.Now:HHmmss}";

        await Page.GetByTestId("job-title-input").FillAsync(jobTitle).ConfigureAwait(false);
        await Page.GetByTestId("job-description-input")
            .FillAsync("Vaga criada por teste E2E automatizado.").ConfigureAwait(false);
        await Page.GetByTestId("job-type-select").SelectOptionAsync("Remote").ConfigureAwait(false);

        var submit = Page.GetByTestId("job-create-submit");
        Assert.True(await submit.IsEnabledAsync().ConfigureAwait(false),
            "Botão de criação de vaga deveria estar habilitado");
        await submit.ClickAsync().ConfigureAwait(false);
        var jobModal = Page.GetByTestId("job-details-modal");
        await jobModal.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible
        }).ConfigureAwait(false);
        Assert.Equal(jobTitle, (await jobModal.Locator("#job-modal-title").TextContentAsync().ConfigureAwait(false))?.Trim());
    }

    [Fact]
    public async Task Advertiser_ReceivedApplications_ShouldLoad()
    {
        await LoginAsAdvertiserAsync().ConfigureAwait(false);

        var response = await GotoAsync("/received-applications").ConfigureAwait(false);
        Assert.True(response!.Ok, $"ReceivedApplications retornou {response.Status}");
        await WaitForBlazorAsync().ConfigureAwait(false);
    }

    [Fact]
    public async Task Advertiser_Dashboard_ShouldLoad()
    {
        await LoginAsAdvertiserAsync().ConfigureAwait(false);

        var response = await GotoAsync("/dashboard").ConfigureAwait(false);
        Assert.True(response!.Ok, $"Dashboard retornou {response.Status}");
        await WaitForBlazorAsync().ConfigureAwait(false);

        var body = await Page.Locator("body").InnerTextAsync().ConfigureAwait(false);
        Assert.Contains("SEU ESPAÇO RECRUIVA", body);
        Assert.Contains("Total de vagas", body);
        Assert.DoesNotContain("SUA JORNADA", body);
    }

    [Fact]
    public async Task Advertiser_MySubscription_ShouldLoad()
    {
        await LoginAsAdvertiserAsync().ConfigureAwait(false);

        var response = await GotoAsync("/my-subscription").ConfigureAwait(false);
        Assert.True(response!.Ok, $"MySubscription retornou {response.Status}");
        await WaitForBlazorAsync().ConfigureAwait(false);
    }

    [Fact]
    public async Task Advertiser_UploadLogo_PageShouldLoad()
    {
        await LoginAsAdvertiserAsync().ConfigureAwait(false);

        var response = await GotoAsync("/advertiser/upload-logo").ConfigureAwait(false);
        Assert.True(response!.Ok, $"UploadLogo retornou {response.Status}");
        await WaitForBlazorAsync().ConfigureAwait(false);
    }

    [Fact]
    public async Task Address_Create_ShouldRenderForm()
    {
        await LoginAsAdvertiserAsync().ConfigureAwait(false);

        var response = await GotoAsync("/address/create").ConfigureAwait(false);
        Assert.True(response!.Ok, $"AddressCreate retornou {response.Status}");
        await WaitForBlazorAsync().ConfigureAwait(false);
    }
}
