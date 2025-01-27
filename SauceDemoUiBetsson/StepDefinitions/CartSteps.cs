using NUnit.Framework;
using SauceDemoUiBetsson.Drivers;
using SauceDemoUiBetsson.Pages;
using TechTalk.SpecFlow;

namespace SauceDemoUiBetsson.StepDefinitions;

[Binding]
public class CartSteps(BrowserDriver browserDriver)
{
    private readonly CartPage _cartPage = new(browserDriver.Page);
    private readonly LoginPage _loginPage = new(browserDriver.Page);

    [Given(@"I am logged in as ""(.*)""")]
    public async Task GivenIAmLoggedInAs(string username)
    {
        await _loginPage.Page.GotoAsync("https://www.saucedemo.com");
        await _loginPage.Login(username, "secret_sauce");
        await _cartPage.WaitForPageLoad();
    }
    
    [Given(@"I have added ""(.*)"" to the cart")]
    public async Task GivenIHaveAddedToTheCart(string itemName)
    {
        await _cartPage.AddItemToCart(itemName);
        var count = await _cartPage.GetCartItemCount();
        Assert.That(count, Is.GreaterThan(0), "Item was not added to cart");
    }

    [When(@"I add ""(.*)"" to the cart")]
    public async Task WhenIAddToTheCart(string itemName)
    {
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
        Assert.That(await _cartPage.IsCartEmpty(), Is.True, "Cart is not empty");
    }
}