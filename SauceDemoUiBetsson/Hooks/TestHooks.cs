using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using SauceDemoUiBetsson.Drivers;
using SauceDemoUiBetsson.Models;
using SauceDemoUiBetsson.Utilities;
using Serilog;
using TechTalk.SpecFlow;

namespace SauceDemoUiBetsson.Hooks;

[Binding]
public class TestHooks(
    BrowserDriver browserDriver,
    ScenarioContext scenarioContext,
    FeatureContext featureContext,
    TestSettings settings)
{
    [BeforeScenario]
    public async Task BeforeScenario()
    {
        Log.Information($"Starting scenario: {scenarioContext.ScenarioInfo.Title}");
        Log.Information($"Navigating to base URL: {settings.BaseUrl}");
        await browserDriver.Page.GotoAsync(settings.BaseUrl);
    }
    
    [AfterScenario]
    public async Task AfterScenario()
    {
        try
        {
            if (scenarioContext.TestError != null)
            {
                // Initialize and configure the logger only on failure
                LoggerConfig.ConfigureLogging();

                // Log the error details
                Log.Error(scenarioContext.TestError, 
                    $"Scenario failed: {scenarioContext.ScenarioInfo.Title}\nError: {scenarioContext.TestError.Message}");

                // Capture screenshot before closing the browser
                await CaptureScreenshot();
            }
            else
            {
                Log.Information($"Scenario completed successfully: {scenarioContext.ScenarioInfo.Title}");
            }
        }
        catch (Exception ex)
        {
            // Log unexpected errors in the hook itself
            Log.Error(ex, "Error in AfterScenario hook");
        }
        finally
        {
            // Ensure resources are cleaned up
            await browserDriver.Page.CloseAsync();
            browserDriver.Dispose();
        }
    }
    
    private async Task CaptureScreenshot()
    {
        try
        {
            const string screenshotDirectory = "Screenshots";
            var screenshotFileName = $"{featureContext.FeatureInfo.Title}_{scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var screenshotPath = Path.Combine(screenshotDirectory, screenshotFileName);
            
            Directory.CreateDirectory(screenshotDirectory);
            
            await browserDriver.Page.ScreenshotAsync(new PageScreenshotOptions
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