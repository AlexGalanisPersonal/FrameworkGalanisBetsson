using SauceDemoUiBetsson.Drivers;
using SauceDemoUiBetsson.Pages;
using TechTalk.SpecFlow;

namespace SauceDemoUiBetsson.StepDefinitions;

[Binding]
public class LoginSteps(BrowserDriver browserDriver)
{
    private readonly LoginPage _loginPage = new(browserDriver.Page);

    [Given(@"I am on the login page")]
    public async Task GivenIAmOnTheLoginPage()
    {
        await _loginPage.Page.GotoAsync("https://www.saucedemo.com");
        await _loginPage.WaitForPageLoad();
        Assert.That(await _loginPage.IsOnLoginPage(), Is.True, "Not on login page");
    }
    
    [When(@"I enter valid username ""(.*)"" and password ""(.*)""")]
    public async Task WhenIEnterValidUsernameAndPassword(string username, string password)
    {
        await _loginPage.Login(username, password);
    }

    [When(@"I enter invalid username ""(.*)"" and password ""(.*)""")]
    public async Task WhenIEnterInvalidUsernameAndPassword(string username, string password)
    {
        await _loginPage.Login(username, password);
    }

    [When(@"I enter locked out username ""(.*)"" and password ""(.*)""")]
    public async Task WhenIEnterLockedOutUsernameAndPassword(string username, string password)
    {
        await _loginPage.Login(username, password);
    }
    
    [Then(@"I should be redirected to the inventory page")]
    public async Task ThenIShouldBeRedirectedToTheInventoryPage()
    {
        await _loginPage.Page.WaitForURLAsync("**/inventory.html");
    }

    [Then(@"I should see an error message")]
    public async Task ThenIShouldSeeAnErrorMessage()
    {
        var errorMessage = await _loginPage.GetErrorMessage();
        Assert.That(errorMessage, Contains.Substring("Username and password do not match"));
    }

    [Then(@"I should see a locked out error message")]
    public async Task ThenIShouldSeeALockedOutErrorMessage()
    {
        var errorMessage = await _loginPage.GetErrorMessage();
        Assert.That(errorMessage, Contains.Substring("Sorry, this user has been locked out"));
    }
}