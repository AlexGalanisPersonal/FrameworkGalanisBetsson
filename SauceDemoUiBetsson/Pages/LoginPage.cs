using Microsoft.Playwright;

namespace SauceDemoUiBetsson.Pages;

public sealed class LoginPage(IPage page) : BasePage(page)
{
    private const string UsernameInput = "#user-name";
    private const string PasswordInput = "#password";
    private const string LoginButton = "#login-button";
    private const string ErrorMessage = "[data-test='error']";

    public async Task WaitForPageLoad()
    {
        await Page.WaitForSelectorAsync(UsernameInput);
    }
    
    public async Task Login(string username, string password)
    {
        await WaitAndFill(UsernameInput, username);
        await WaitAndFill(PasswordInput, password);
        await WaitAndClick(LoginButton);
    }
    
    public async Task<string?> GetErrorMessage()
    {
        return await WaitAndGetText(ErrorMessage);
    }

    public async Task<bool> IsOnLoginPage()
    {
        return await Page.IsVisibleAsync(LoginButton);
    }
}