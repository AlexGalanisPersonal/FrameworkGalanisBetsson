using SauceDemoUiBetsson.Utilities;
using TechTalk.SpecFlow;

namespace SauceDemoUiBetsson.StepDefinitions;

[Binding]
public class CommonSteps(LoginHelper loginHelper)
{
    [Given(@"I am logged in as ""(.*)""")]
    public async Task GivenIAmLoggedInAs(string userType)
    {
        await loginHelper.LoginAs(userType);
    }
}