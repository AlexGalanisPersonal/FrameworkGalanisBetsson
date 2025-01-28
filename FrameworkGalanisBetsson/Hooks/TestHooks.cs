using BoDi;
using SauceDemoUiBetsson.Drivers;
using SauceDemoUiBetsson.Models;
using SauceDemoUiBetsson.UiSauceDemo.Utilities;
using Serilog;
using TechTalk.SpecFlow;

namespace SauceDemoUiBetsson.Hooks;

[Binding]
public class TestHooks(
    ScenarioContext scenarioContext,
    FeatureContext featureContext,
    TestSettings settings,
    IObjectContainer objectContainer)
{
    [BeforeScenario("ui")]
    public async Task BeforeUiScenario()
    {
        if (objectContainer.IsRegistered<BrowserDriver>())
        {
            var browserDriver = objectContainer.Resolve<BrowserDriver>();
            Log.Information($"Starting UI scenario: {scenarioContext.ScenarioInfo.Title}");
            Log.Information($"Navigating to base URL: {settings.BaseUrl}");
            await browserDriver.Page.GotoAsync(settings.BaseUrl);
        }
    }

    [BeforeScenario("api")]
    public void BeforeApiScenario()
    {
        Log.Information($"Starting API scenario: {scenarioContext.ScenarioInfo.Title}");
    }
    
    [AfterScenario]
    public async Task AfterScenario()
    {
        try
        {
            if (scenarioContext.TestError != null)
            {
                LoggerConfig.ConfigureLogging();
                Log.Error(scenarioContext.TestError, 
                    $"Scenario failed: {scenarioContext.ScenarioInfo.Title}\nError: {scenarioContext.TestError.Message}");

                // Only attempt screenshot for UI tests with a browser
                if (scenarioContext.ScenarioInfo.Tags.Contains("ui") && 
                    objectContainer.IsRegistered<BrowserDriver>())
                {
                    await CaptureScreenshot();
                }
            }
            else
            {
                Log.Information($"Scenario completed successfully: {scenarioContext.ScenarioInfo.Title}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in AfterScenario hook");
        }
    }
    
    private async Task CaptureScreenshot()
    {
        try
        {
            var browserDriver = objectContainer.Resolve<BrowserDriver>();
            
            const string screenshotDirectory = "Screenshots";
            var screenshotFileName = $"{featureContext.FeatureInfo.Title}_{scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var screenshotPath = Path.Combine(screenshotDirectory, screenshotFileName);
            
            Directory.CreateDirectory(screenshotDirectory);
            
            await browserDriver.Page.ScreenshotAsync(new()
            {
                FullPage = true,
                Path = screenshotPath
            });
            
            Log.Information($"Screenshot saved to: {screenshotPath}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to capture screenshot");
        }
    }
    
    [AfterTestRun]
    public static void CleanupLogger()
    {
        Log.Information("Test run completed. Cleaning up logger.");
        Log.CloseAndFlush();
    }
}