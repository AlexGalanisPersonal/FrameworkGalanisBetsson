using Microsoft.Playwright;
using Serilog;

namespace SauceDemoUiBetsson.Pages;

public abstract class BasePage
{
    public readonly IPage Page;
    private readonly ILogger _logger;

    protected BasePage(IPage page)
    {
        Page = page;
        _logger = Log.ForContext(GetType());
    }

    protected async Task WaitAndClick(string selector)
    {
        try
        {
            _logger.Information($"Attempting to click element: {selector}");
            await Page.WaitForSelectorAsync(selector);
            await Page.ClickAsync(selector);
            _logger.Information($"Successfully clicked element: {selector}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Failed to click element: {selector}");
            throw;
        }
    }
    
    protected async Task WaitAndFill(string selector, string value)
    {
        try
        {
            _logger.Information($"Attempting to fill element: {selector} with value: {value}");
            await Page.WaitForSelectorAsync(selector);
            await Page.FillAsync(selector, value);
            _logger.Information($"Successfully filled element: {selector}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Failed to fill element: {selector}");
            throw;
        }
    }
    
    protected async Task<string?> WaitAndGetText(string selector)
    {
        try
        {
            _logger.Information($"Attempting to get text from element: {selector}");
            await Page.WaitForSelectorAsync(selector);
            var text = await Page.TextContentAsync(selector);
            _logger.Information($"Successfully got text from element: {selector}, value: {text}");
            return text;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Failed to get text from element: {selector}");
            throw;
        }
    }
}