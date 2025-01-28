using SauceDemoUiBetsson.Drivers;
using SauceDemoUiBetsson.Models;
using SauceDemoUiBetsson.Pages;
using SauceDemoUiBetsson.UiSauceDemo.Pages;

namespace SauceDemoUiBetsson.Utilities;

public class LoginHelper(
    BrowserDriver browserDriver,
    NavigationHelper navigationHelper,
    TestSettings settings)
{
    private readonly LoginPage _loginPage = new(browserDriver.Page);

    public async Task LoginAs(string userType)
    {
        await navigationHelper.GoToLogin();

        // Handle invalid user case
        if (userType == "invalid")
        {
            await _loginPage.Login("invalid_user", "invalid_pass");
            return;
        }

        // Handle valid user types
        if (!settings.Users.TryGetValue(userType, out var credentials))
        {
            throw new ArgumentException($"User type '{userType}' not found");
        }

        await _loginPage.Login(credentials.Username, credentials.Password);

        // Only wait for inventory page navigation for standard successful login
        if (userType == "standard")
        {
            await _loginPage.WaitForInventoryPage();
        }
    }
}