using Microsoft.Playwright;

namespace SauceDemoUiBetsson.UiSauceDemo.Pages;

public sealed class LoginPage(IPage page) : BasePage(page)
{
    private readonly ILocator _usernameInput = page.Locator("[data-test='username']");
    private readonly ILocator _passwordInput = page.Locator("[data-test='password']");
    private readonly ILocator _loginButton = page.Locator("[data-test='login-button']");
    private readonly ILocator _errorMessage = page.Locator("[data-test='error']");

    public async Task WaitForPageLoad()
    {
        await _usernameInput.WaitForAsync();
    }
    public async Task WaitForInventoryPage()
    {
        await Page.WaitForURLAsync("**/inventory.html");
    }
    public async Task Login(string username, string password)
    {
        await _usernameInput.FillAsync(username);
        await _passwordInput.FillAsync(password);
        await _loginButton.ClickAsync();
    }
    
    public async Task<string?> GetErrorMessage()
    {
        await _errorMessage.WaitForAsync();
        return await _errorMessage.TextContentAsync();
    }

    public async Task<bool> IsOnLoginPage()
    {
        return await _loginButton.IsVisibleAsync();
    }
}