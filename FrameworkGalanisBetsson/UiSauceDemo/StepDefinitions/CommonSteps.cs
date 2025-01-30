using SauceDemoUiBetsson.UiSauceDemo.Utilities;
using TechTalk.SpecFlow;

namespace SauceDemoUiBetsson.UiSauceDemo.StepDefinitions;

[Binding]
public class CommonSteps(LoginHelper loginHelper)
{
    [Given(@"I am logged in as ""(.*)""")]
    public async Task GivenIAmLoggedInAs(string userType)
    {
        await loginHelper.LoginAs(userType);
    }
}