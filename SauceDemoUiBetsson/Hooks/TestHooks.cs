using Microsoft.Playwright;
using SauceDemoUiBetsson.Drivers;
using SauceDemoUiBetsson.Utilities;
using Serilog;
using TechTalk.SpecFlow;

namespace SauceDemoUiBetsson.Hooks;

[Binding]
public class TestHooks(BrowserDriver browserDriver, ScenarioContext scenarioContext, FeatureContext featureContext)
{
    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        LoggerConfig.ConfigureLogging();
        Log.Information("Starting test execution");
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        Log.Information($"Starting scenario: {scenarioContext.ScenarioInfo.Title}");
        await browserDriver.Page.GotoAsync("https://www.saucedemo.com");
    }
    
    [AfterScenario]
    public async Task AfterScenario()
    {
        if (scenarioContext.TestError != null)
        {
            Log.Error(scenarioContext.TestError, "Scenario failed");
            
            var screenshotPath = Path.Combine("Screenshots", 
                $"{featureContext.FeatureInfo.Title}_{scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            
            Directory.CreateDirectory("Screenshots");
            
            await browserDriver.Page.ScreenshotAsync(new PageScreenshotOptions
            {
                FullPage = true,
                Path = screenshotPath
            });
            
            Log.Information($"Screenshot saved to: {screenshotPath}");
        }
        else
        {
            Log.Information($"Scenario completed successfully: {scenarioContext.ScenarioInfo.Title}");
        }
        
        await browserDriver.Page.CloseAsync();
    }
    
    [AfterTestRun]
    public static void AfterTestRun()
    {
        Log.Information("Test execution completed");
        Log.CloseAndFlush();
    }
}