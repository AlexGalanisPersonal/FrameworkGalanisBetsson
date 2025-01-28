using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using SauceDemoUiBetsson.Drivers;
using SauceDemoUiBetsson.Models;
using SauceDemoUiBetsson.Pages;
using TechTalk.SpecFlow;
using SauceDemoUiBetsson.Utilities;

namespace SauceDemoUiBetsson.StepDefinitions;

[Binding]
public class CartSteps
{
    private readonly CartPage _cartPage;
    private readonly TestSettings _settings;
    private readonly IPage _page;
    
    private readonly ScenarioContext _scenarioContext;

    public CartSteps(BrowserDriver browserDriver, IConfiguration configuration, ScenarioContext scenarioContext, NavigationHelper navigationHelper, LoginHelper loginHelper)
    {
        _page = browserDriver.Page;
        _cartPage = new CartPage(_page);
        new LoginPage(_page);
        _settings = configuration.GetSection("TestSettings").Get<TestSettings>()
            ?? throw new InvalidOperationException("Test settings not found");
        _scenarioContext = scenarioContext;
    }

    [Given(@"I have added items to cart:")]
    public async Task GivenIHaveAddedItemsToCart(Table table)
    {
        var prices = new Dictionary<string, decimal>();
        
        foreach (var row in table.Rows)
        {
            var itemName = row["Item Name"];
            var price = await _cartPage.GetItemPrice(itemName);
            prices[itemName] = price;
            await _cartPage.AddItemToCart(itemName);
        }
        
        _scenarioContext[ScenarioContextKeys.ItemPrices] = prices;
        
        var itemCount = await _cartPage.GetCartItemCount();
        Assert.That(itemCount, Is.EqualTo(table.Rows.Count));
    }
    
    [Given(@"I have added ""(.*)"" to the cart")]
    public async Task GivenIHaveAddedToTheCart(string itemName)
    {
        var price = await _cartPage.GetItemPrice(itemName);
        var prices = new Dictionary<string, decimal> { { itemName, price } };
        _scenarioContext[ScenarioContextKeys.ItemPrices] = prices;
        await _cartPage.AddItemToCart(itemName);
        
        var count = await _cartPage.GetCartItemCount();
        Assert.That(count, Is.GreaterThan(0));
    }

    [When(@"I add ""(.*)"" to the cart")]
    public async Task WhenIAddToTheCart(string itemName)
    {
        // Get existing prices or create new dictionary if it doesn't exist
        var prices = _scenarioContext.ContainsKey(ScenarioContextKeys.ItemPrices) 
            ? _scenarioContext.Get<Dictionary<string, decimal>>(ScenarioContextKeys.ItemPrices)
            : new Dictionary<string, decimal>();

        var price = await _cartPage.GetItemPrice(itemName);
        prices[itemName] = price;
        _scenarioContext[ScenarioContextKeys.ItemPrices] = prices;
        await _cartPage.AddItemToCart(itemName);
    }

    [When(@"I remove ""(.*)"" from the cart")]
    public async Task WhenIRemoveFromTheCart(string itemName)
    {
        await _cartPage.RemoveItemFromCart(itemName);
    }

    [Then(@"the cart count should be (.*)")]
    public async Task ThenTheCartCountShouldBe(int expectedCount)
    {
        var actualCount = await _cartPage.GetCartItemCount();
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }

    [Then(@"the cart should be empty")]
    public async Task ThenTheCartShouldBeEmpty()
    {
        var isEmpty = await _cartPage.IsCartEmpty();
        Assert.That(isEmpty, Is.True, "Cart is not empty");
    }
}