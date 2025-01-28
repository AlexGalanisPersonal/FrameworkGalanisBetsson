using Microsoft.Playwright;
using SauceDemoUiBetsson.Drivers;
using SauceDemoUiBetsson.Pages;
using SauceDemoUiBetsson.Utilities;
using TechTalk.SpecFlow;

namespace SauceDemoUiBetsson.StepDefinitions;

[Binding]
public class CheckoutSteps
{
    private readonly CheckoutPage _checkoutPage;
    private readonly CartPage _cartPage;
    private readonly IPage _page;
    private readonly ScenarioContext _scenarioContext;
    private readonly NavigationHelper _navigationHelper;
    private decimal _subtotal;
    private decimal _tax;

    public CheckoutSteps(BrowserDriver browserDriver, ScenarioContext scenarioContext, NavigationHelper navigationHelper)
    {
        _page = browserDriver.Page;
        _checkoutPage = new CheckoutPage(_page);
        _cartPage = new CartPage(_page);
        _scenarioContext = scenarioContext;
        _navigationHelper = navigationHelper;
    }

    [Given(@"I navigate to the checkout page")]
    public async Task GivenINavigateToTheCheckoutPage()
    {
        await _cartPage.OpenCart();
        await _checkoutPage.StartCheckout();
    }

    [When(@"I enter the following customer details:")]
    public async Task WhenIEnterTheFollowingCustomerDetails(Table table)
    {
        var details = table.Rows[0];
        await _checkoutPage.EnterCustomerDetails(
            details["First Name"],
            details["Last Name"],
            details["Zip Code"]
        );
    }

    [When(@"I proceed with empty fields")]
    public async Task WhenIProceedWithEmptyFields()
    {
        await _checkoutPage.ClearAllFields();
        await _checkoutPage.ContinueCheckout();
    }

    [When(@"I fill in ""(.*)"" with ""(.*)""")]
    public async Task WhenIFillInWith(string field, string value)
    {
        await _checkoutPage.FillField(field, value);
    }

    [When(@"I enter the customer information")]
    public async Task WhenIEnterTheCustomerInformation()
    {
        await _checkoutPage.EnterCustomerDetails("Alex", "Galanis", "15772");
    }

    [When(@"I proceed to checkout overview")]
    public async Task WhenIProceedToCheckoutOverview()
    {
        await _checkoutPage.ContinueCheckout();
    }

    [When(@"I complete the checkout")]
    public async Task WhenICompleteTheCheckout()
    {
        await _checkoutPage.FinishCheckout();
    }

    [When(@"I cancel the checkout")]
    public async Task WhenICancelTheCheckout()
    {
        await _checkoutPage.CancelCheckout();
    }

    [Then(@"I should see the error message ""(.*)""")]
    public async Task ThenIShouldSeeTheErrorMessage(string expectedError)
    {
        var actualError = await _checkoutPage.GetErrorMessage();
        Assert.That(actualError, Is.EqualTo(expectedError));
    }

    [Then(@"I should see the correct item prices in the summary")]
    public async Task ThenIShouldSeeTheCorrectItemPricesInTheSummary()
    {
        var itemPrices = _scenarioContext.Get<Dictionary<string, decimal>>(ScenarioContextKeys.ItemPrices);
        var itemDetails = await _checkoutPage.GetCartItemDetails();
        
        foreach (var detail in itemDetails)
        {
            var expectedPrice = itemPrices[detail.Name];
            var actualPrice = decimal.Parse(detail.Price.Replace("$", ""));
            Assert.That(actualPrice, Is.EqualTo(expectedPrice));
        }
        
        _subtotal = itemPrices.Values.Sum();
    }

    [Then(@"the tax amount should be 8% of the subtotal")]
    public async Task ThenTheTaxAmountShouldBePercentOfTheSubtotal()
    {
        var subtotal = _scenarioContext.Get<decimal>("Subtotal");
        var expectedTax = Math.Round(subtotal * 0.08m, 2);
        var actualTax = _scenarioContext["Tax"];
    
        Assert.That(actualTax, Is.EqualTo(expectedTax));
    }

    [Then(@"the total should be the sum of subtotal and tax")]
    public async Task ThenTheTotalShouldBeTheSumOfSubtotalAndTax()
    {
        var subtotal = _scenarioContext.Get<decimal>("Subtotal");
        var tax = _scenarioContext.Get<decimal>("Tax");
        var expectedTotal = subtotal + tax;
    
        var actualTotalStr = await _checkoutPage.GetTotalAmount();
        var actualTotal = decimal.Parse(actualTotalStr?.Replace("$", "") ?? "0");
    
        Assert.That(actualTotal, Is.EqualTo(expectedTotal));
    }

    [Then(@"I should see the order confirmation")]
    public async Task ThenIShouldSeeTheOrderConfirmation()
    {
        var message = await _checkoutPage.GetSuccessMessage();
        Assert.That(message.Trim().ToLower(), Is.EqualTo("thank you for your order!"));
    }

    [Then(@"I should return to the inventory page")]
    public async Task ThenIShouldReturnToTheInventoryPage()
    {
        await _page.WaitForURLAsync("**/inventory.html");
    }

    [Then(@"the cart should have (.*) items")]
    public async Task ThenTheCartShouldHaveItems(int expectedCount)
    {
        var actualCount = await _cartPage.GetCartItemCount();
        Assert.That(actualCount, Is.EqualTo(expectedCount));
    }
    [When(@"I complete checkout information")]
    public async Task WhenICompleteCheckoutInformation()
    {
        await _checkoutPage.EnterCustomerDetails("Alex", "Galanis", "15772");
        await _checkoutPage.ContinueCheckout();
        var items = await _checkoutPage.GetCartItemDetails();
        var subtotal = items.Sum(item => decimal.Parse(item.Price.Replace("$", "")));
        _scenarioContext["Subtotal"] = subtotal;
        
        var actualTaxStr = await _checkoutPage.GetTaxAmount();
        var actualTax = decimal.Parse(actualTaxStr?.Replace("$", "") ?? "0");
        _scenarioContext["Tax"] = actualTax;
    }

    [Given(@"I am on the checkout overview page")]
    public async Task GivenIAmOnTheCheckoutOverviewPage()
    {
        await _navigationHelper.GoToCart();
        await _checkoutPage.StartCheckout();
        await WhenICompleteCheckoutInformation();
    }

    [Then(@"I proceed to checkout overview successfully")]
    public async Task ThenIProceedToCheckoutOverviewSuccessfully()
    {
        await _checkoutPage.ContinueCheckout();
        Assert.That(await _checkoutPage.IsOnOverviewPage(), Is.True);
    }
}