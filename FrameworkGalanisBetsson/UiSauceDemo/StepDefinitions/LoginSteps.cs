using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using SauceDemoUiBetsson.Drivers;
using SauceDemoUiBetsson.Models;
using SauceDemoUiBetsson.Pages;
using SauceDemoUiBetsson.UiSauceDemo.Pages;
using SauceDemoUiBetsson.Utilities;
using TechTalk.SpecFlow;

namespace SauceDemoUiBetsson.UiSauceDemo.StepDefinitions;

[Binding]
public class LoginSteps
{
    private readonly LoginPage _loginPage;
    private readonly TestSettings _settings;
    private readonly IPage _page;
    private readonly LoginHelper _loginHelper;
    private readonly NavigationHelper _navigationHelper;

    public LoginSteps(BrowserDriver browserDriver, IConfiguration configuration, LoginHelper loginHelper, NavigationHelper navigationHelper)
    {
        _loginHelper = loginHelper;
        _navigationHelper = navigationHelper;
        _page = browserDriver.Page;
        _loginPage = new LoginPage(_page);
        _settings = configuration.GetSection("TestSettings").Get<TestSettings>()
                    ?? throw new InvalidOperationException("Test settings not found");
    }

    [Given(@"I am on the login page")]
    public async Task GivenIAmOnTheLoginPage()
    {
        await _navigationHelper.GoToLogin();
        await _loginPage.WaitForPageLoad();
    }

    [When(@"I log in as ""(.*)""")]
    public async Task WhenILogInAs(string userType)
    {
        await _loginHelper.LoginAs(userType);
    }

    [Then(@"I should be redirected to the inventory page")]
    public async Task ThenIShouldBeRedirectedToTheInventoryPage()
    {
        await _page.WaitForURLAsync("**/inventory.html");
    }

    [Then(@"I should see an error message ""(.*)""")]
    public async Task ThenIShouldSeeAnErrorMessage(string expectedError)
    {
        var errorMessage = await _loginPage.GetErrorMessage();
        Assert.That(errorMessage, Is.EqualTo(expectedError));
    }
}