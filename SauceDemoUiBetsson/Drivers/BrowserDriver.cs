using Microsoft.Playwright;

namespace SauceDemoUiBetsson.Drivers;

public class BrowserDriver : IDisposable
{
    private readonly IPlaywright _playwright;
    private readonly IBrowser _browser;
    private readonly IBrowserContext _context;

    public IPage Page { get; }

    public BrowserDriver()
    {
        _playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
        _browser = _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo= 50
        }).GetAwaiter().GetResult();
        
        _context = _browser.NewContextAsync().GetAwaiter().GetResult();
        Page = _context.NewPageAsync().GetAwaiter().GetResult();
    }
    
    public void Dispose()
    {
        Page?.CloseAsync().GetAwaiter().GetResult();
        _context?.CloseAsync().GetAwaiter().GetResult();
        _browser?.CloseAsync().GetAwaiter().GetResult();
        _playwright?.Dispose();
    }
}