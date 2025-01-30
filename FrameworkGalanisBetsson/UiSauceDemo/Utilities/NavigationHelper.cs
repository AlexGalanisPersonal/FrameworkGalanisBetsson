using SauceDemoUiBetsson.Drivers;
using SauceDemoUiBetsson.Models;

namespace SauceDemoUiBetsson.UiSauceDemo.Utilities;
public class NavigationHelper(BrowserDriver browserDriver, TestSettings settings)
{
    public async Task GoToLogin() => await browserDriver.Page.GotoAsync(settings.BaseUrl);
    public async Task GoToCart() => await browserDriver.Page.GotoAsync($"{settings.BaseUrl}/cart.html");
}