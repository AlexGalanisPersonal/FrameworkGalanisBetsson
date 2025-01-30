using BoDi;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using SauceDemoUiBetsson.ApiPetStore.Helpers;
using SauceDemoUiBetsson.Drivers;
using SauceDemoUiBetsson.Models;
using SauceDemoUiBetsson.UiSauceDemo.Utilities;
using TechTalk.SpecFlow;

namespace SauceDemoUiBetsson.Hooks;

[Binding]
public class DependencyInjectionHooks(IObjectContainer objectContainer)
{
    private IPlaywright? _playwright;
    private IAPIRequestContext? _apiRequestContext;

    [BeforeScenario(Order = 0)]
    public void RegisterCommonServices()
    {
        // Register configuration for both UI and API tests
        var configuration = ConfigurationHelper.GetConfiguration();
        objectContainer.RegisterInstanceAs<IConfiguration>(configuration);

        // Register settings for UI tests
        var settings = configuration.GetSection("TestSettings").Get<TestSettings>();
        if (settings == null) throw new InvalidOperationException("Test settings not found");
        objectContainer.RegisterInstanceAs(settings);
    }

    [BeforeScenario("ui", Order = 1)]
    public void RegisterUiServices()
    {
        // Create browser driver in non-headless mode for UI tests
        var browserDriver = new BrowserDriver(headless: false);
        objectContainer.RegisterInstanceAs(browserDriver);

        objectContainer.RegisterTypeAs<NavigationHelper, NavigationHelper>();
        objectContainer.RegisterTypeAs<LoginHelper, LoginHelper>();
    }

    [BeforeScenario("api", Order = 1)]
    public async Task RegisterApiServices()
    {
        var configuration = objectContainer.Resolve<IConfiguration>();
        var baseUrl = configuration["ApiSettings:BaseUrl"] 
            ?? throw new InvalidOperationException("API base URL not found in configuration");
        
        // For API tests, only create the API request context
        _playwright = await Playwright.CreateAsync();
        _apiRequestContext = await _playwright.APIRequest.NewContextAsync();

        var apiClient = new ApiClient(_apiRequestContext, baseUrl);
        objectContainer.RegisterInstanceAs(apiClient);
    }

    [AfterScenario("ui")]
    public void DisposeUiServices()
    {
        if (!objectContainer.IsRegistered<BrowserDriver>()) return;
        var browserDriver = objectContainer.Resolve<BrowserDriver>();
        browserDriver.Dispose();
    }

    [AfterScenario("api")]
    public async Task DisposeApiServices()
    {
        if (_apiRequestContext != null)
        {
            await _apiRequestContext.DisposeAsync();
        }
        _playwright?.Dispose();
    }
}