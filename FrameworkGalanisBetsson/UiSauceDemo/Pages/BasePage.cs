using Microsoft.Playwright;
using Serilog;

namespace SauceDemoUiBetsson.UiSauceDemo.Pages;

public abstract class BasePage
{
    public readonly IPage Page;

    protected BasePage(IPage page)
    {
        Page = page;
        Log.ForContext(GetType());
    }
}