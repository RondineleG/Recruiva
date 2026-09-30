namespace Recruiva.Web.Data;

public static class DevelopmentIdentitySeedData
{
    private const string Password = "Recruiva123";

    private static readonly string[] Emails =
    [
        "contato@techsolutions.com",
        "rh@marketingdigital.com",
        "joao.silva@email.com",
        "maria.santos@email.com"
    ];

    public static async Task InitializeAsync(UserManager<ApplicationUser> userManager)
    {
        foreach (var email in Emails)
        {
            var user = await userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user is not null)
            {
                var resetToken = await userManager.GeneratePasswordResetTokenAsync(user)
                    .ConfigureAwait(false);
                var resetResult = await userManager.ResetPasswordAsync(user, resetToken, Password)
                    .ConfigureAwait(false);
                if (!resetResult.Succeeded)
                {
                    var errors = string.Join(", ", resetResult.Errors.Select(error => error.Description));
                    throw new InvalidOperationException(
                        $"Não foi possível atualizar o usuário de desenvolvimento {email}: {errors}");
                }

                await userManager.SetLockoutEndDateAsync(user, null).ConfigureAwait(false);
                await userManager.ResetAccessFailedCountAsync(user).ConfigureAwait(false);
                continue;
            }

            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var result = await userManager.CreateAsync(user, Password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                throw new InvalidOperationException(
                    $"Não foi possível criar o usuário de desenvolvimento {email}: {errors}");
            }

            await userManager.SetLockoutEnabledAsync(user, false).ConfigureAwait(false);
        }
    }
}
