using Microsoft.Playwright;

namespace Recruiva.E2ETests;

public class AuthenticationTests : E2ETestBase
{
    public AuthenticationTests(PlaywrightFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Register_NewUser_ShouldAutoLogin()
    {
        var email = $"e2e-{Guid.NewGuid():N}@test.com";

        var registered = await TryRegisterAsync(email, TestPassword).ConfigureAwait(false);
        Assert.True(registered, "Registro não redirecionou para fora da página");

        await GotoAsync("/dashboard").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);
        Assert.True(await Page.GetByTestId("nav-applications").IsVisibleAsync().ConfigureAwait(false));
        await AssertLoggedInAsync().ConfigureAwait(false);
    }

    [Fact]
    public async Task Register_Advertiser_ShouldOpenRecruiterWorkspace()
    {
        var email = $"recrutador-{Guid.NewGuid():N}@test.com";

        var registered = await TryRegisterAsync(email, TestPassword, accountType: "advertiser").ConfigureAwait(false);
        Assert.True(registered, "Cadastro de recrutador não concluiu");

        await GotoAsync("/dashboard").ConfigureAwait(false);
        await WaitForBlazorAsync().ConfigureAwait(false);
        Assert.True(await Page.GetByTestId("nav-my-jobs").IsVisibleAsync().ConfigureAwait(false));
        Assert.Contains("SEU ESPAÇO RECRUIVA", await Page.Locator("body").InnerTextAsync().ConfigureAwait(false));
    }

    [Fact]
    public async Task Login_EmptyFields_ShouldShowPortugueseValidation()
    {
        await GotoAsync("/Account/Login").ConfigureAwait(false);
        await Page.GetByTestId("login-submit").ClickAsync().ConfigureAwait(false);

        var body = await Page.Locator("body").InnerTextAsync().ConfigureAwait(false);
        Assert.Contains("Informe seu e-mail.", body);
        Assert.Contains("Informe sua senha.", body);
        Assert.DoesNotContain("The E-mail field is required.", body);
        Assert.DoesNotContain("The Senha field is required.", body);
    }

    [Fact]
    public async Task Register_ShouldShowLoginLinkAndPortugueseValidation()
    {
        await GotoAsync("/Account/Register").ConfigureAwait(false);
        await Page.GetByTestId("register-submit").ClickAsync().ConfigureAwait(false);

        var body = await Page.Locator("body").InnerTextAsync().ConfigureAwait(false);
        Assert.Contains("Já tem uma conta?", body);
        Assert.Contains("Entrar", body);
        Assert.Contains("Informe seu e-mail.", body);
        Assert.Contains("Informe uma senha.", body);
        Assert.DoesNotContain("The E-mail field is required.", body);
        Assert.DoesNotContain("The Senha field is required.", body);
    }

    [Fact]
    public async Task Register_WithMismatchedPassword_ShouldShowError()
    {
        var email = $"e2e-{Guid.NewGuid():N}@test.com";
        var registered = await TryRegisterAsync(email, TestPassword, "OutraSenha9")
            .ConfigureAwait(false);

        Assert.False(registered);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldFail()
    {
        var loggedIn = await TryLoginAsync("naoexiste@test.com", "SenhaErrada1").ConfigureAwait(false);
        Assert.False(loggedIn);
        await AssertLoggedOutAsync().ConfigureAwait(false);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldAuthenticate()
    {
        await LoginOrRegisterAsync(CandidateEmail, TestPassword).ConfigureAwait(false);
        await AssertLoggedInAsync().ConfigureAwait(false);
    }

    [Fact]
    public async Task Login_Form_ShouldAuthenticateThroughBrowser()
    {
        await GotoAsync("/Account/Login").ConfigureAwait(false);
        await Page.GetByTestId("login-email").WaitForAsync().ConfigureAwait(false);
        await Page.GetByTestId("login-email").FillAsync(CandidateEmail).ConfigureAwait(false);
        await Page.GetByTestId("login-password").FillAsync(TestPassword).ConfigureAwait(false);
        await Page.GetByTestId("login-submit").ClickAsync().ConfigureAwait(false);
        await Page.GetByTestId("nav-jobs").WaitForAsync().ConfigureAwait(false);

        Assert.DoesNotContain("/Account/Login", Page.Url, StringComparison.OrdinalIgnoreCase);
        await AssertLoggedInAsync().ConfigureAwait(false);
    }

    [Fact]
    public async Task Anonymous_ShouldRedirect_ToLogin_OnProfilePages()
    {
        await GotoAsync("/profile/candidate").ConfigureAwait(false);
        await Page.GetByTestId("login-email").WaitForAsync().ConfigureAwait(false);
        Assert.Contains("/Account/Login", Page.Url, StringComparison.OrdinalIgnoreCase);
    }
}
