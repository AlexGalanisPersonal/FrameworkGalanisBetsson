using BoDi;
using Microsoft.Extensions.Configuration;
using SauceDemoUiBetsson.Drivers;
using SauceDemoUiBetsson.Models;
using SauceDemoUiBetsson.Utilities;
using TechTalk.SpecFlow;

namespace SauceDemoUiBetsson.Hooks;

[Binding]
public class DependencyInjectionHooks(IObjectContainer objectContainer)
{
    [BeforeScenario(Order = 0)]
    public void RegisterServices()
    {
        // Register IConfiguration
        var configuration = ConfigurationHelper.GetConfiguration();
        objectContainer.RegisterInstanceAs(configuration);

        // Register TestSettings
        var settings = configuration.GetSection("TestSettings").Get<TestSettings>();
        objectContainer.RegisterInstanceAs(settings);

        // Register BrowserDriver
        var browserDriver = new BrowserDriver();
        objectContainer.RegisterInstanceAs(browserDriver);

        // Register Helpers
        objectContainer.RegisterTypeAs<NavigationHelper, NavigationHelper>();
        objectContainer.RegisterTypeAs<LoginHelper, LoginHelper>();
    }
}