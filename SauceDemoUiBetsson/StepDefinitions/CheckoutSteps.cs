using SauceDemoUiBetsson.Drivers;
using SauceDemoUiBetsson.Pages;
using TechTalk.SpecFlow;

namespace SauceDemoUiBetsson.StepDefinitions;

[Binding]
public class CheckoutSteps(BrowserDriver browserDriver)
{
    private readonly CheckoutPage _checkoutPage = new(browserDriver.Page);
    private readonly CartPage _cartPage = new(browserDriver.Page);

    [Given(@"I have the following items in cart:")]
    public async Task GivenIHaveTheFollowingItemsInCart(Table table)
    {
        foreach (var row in table.Rows)
        {
            await _cartPage.AddItemToCart(row["Item Name"]);
        }
        
        // Verify items were added successfully
        var itemCount = await _cartPage.GetCartItemCount();
        Assert.That(itemCount, Is.EqualTo(table.Rows.Count), "Not all items were added to cart");
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

    [When(@"I try to continue with empty fields")]
    public async Task WhenITryToContinueWithEmptyFields()
    {
        await _checkoutPage.ClearAllFields();
        await _checkoutPage.ContinueCheckout();
    }

    [When(@"I enter only the following details:")]
    public async Task WhenIEnterOnlyTheFollowingDetails(Table table)
    {
        await _checkoutPage.ClearAllFields();

        string firstName = "", lastName = "", postalCode = "";

        foreach (var row in table.Rows)
        {
            var field = row["Field"];
            var value = row["Value"];

            switch (field)
            {
                case "First Name":
                    firstName = value;
                    break;
                case "Last Name":
                    lastName = value;
                    break;
                case "Postal Code":
                    postalCode = value;
                    break;
            }
        }

        await _checkoutPage.EnterCustomerDetails(firstName, lastName, postalCode);
    }

    [When(@"I try to continue")]
    public async Task WhenITryToContinue()
    {
        await _checkoutPage.ContinueCheckout();
    }

    [When(@"I enter valid customer information")]
    public async Task WhenIEnterValidCustomerInformation()
    {
        await _checkoutPage.EnterCustomerDetails("John", "Doe", "12345");
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

    [When(@"I click cancel")]
    public async Task WhenIClickCancel()
    {
        await _checkoutPage.CancelCheckout();
    }

    [Then(@"I should see the order confirmation")]
    public async Task ThenIShouldSeeTheOrderConfirmation()
    {
        var message = await _checkoutPage.GetSuccessMessage();
    
        Assert.That(message.Trim().ToLower(), Is.EqualTo("thank you for your order!"),
            $"Order confirmation message does not match. Actual message: '{message}'");
    }

    [Then(@"I should see the error message ""(.*)""")]
    public async Task ThenIShouldSeeTheErrorMessage(string expectedError)
    {
        var actualError = await _checkoutPage.GetErrorMessage();
        Assert.That(actualError, Is.EqualTo(expectedError), 
            $"Expected error message '{expectedError}' but got '{actualError}'");
    }

    [Then(@"I should see the following item details:")]
    public async Task ThenIShouldSeeTheFollowingItemDetails(Table table)
    {
        var actualItems = await _checkoutPage.GetCartItemDetails();
        
        foreach (var item in actualItems)
        {
            Console.WriteLine($"Cart item - Name: {item.Name}, Price: {item.Price}, Quantity: {item.Quantity}");
        }

        foreach (var row in table.Rows)
        {
            var expectedName = row["Item Name"];
            var expectedPrice = row["Price"];
            var expectedQuantity = row["Quantity"];

            var actualItem = actualItems.FirstOrDefault(i => 
                i.Name.Equals(expectedName, StringComparison.OrdinalIgnoreCase));

            Assert.That(actualItem, Is.Not.Null, 
                $"Item '{expectedName}' not found in cart. Available items: {string.Join(", ", actualItems.Select(i => i.Name))}");

            if (actualItem == null) continue;
            Assert.That(actualItem.Price, Is.EqualTo(expectedPrice), 
                $"Price mismatch for item {expectedName}. Expected: {expectedPrice}, Actual: {actualItem.Price}");
            Assert.That(actualItem.Quantity, Is.EqualTo(expectedQuantity), 
                $"Quantity mismatch for item {expectedName}. Expected: {expectedQuantity}, Actual: {actualItem.Quantity}");
        }
    }

    [Then(@"the subtotal should be ""(.*)""")]
    public async Task ThenTheSubtotalShouldBe(string expectedSubtotal)
    {
        var actualSubtotal = await _checkoutPage.GetSubtotal();
        Assert.That(actualSubtotal, Is.EqualTo(expectedSubtotal), 
            $"Expected subtotal {expectedSubtotal} but got {actualSubtotal}");
    }

    [Then(@"the tax should be ""(.*)""")]
    public async Task ThenTheTaxShouldBe(string expectedTax)
    {
        var actualTax = await _checkoutPage.GetTaxAmount();
        Assert.That(actualTax, Is.EqualTo(expectedTax), 
            $"Expected tax {expectedTax} but got {actualTax}");
    }

    [Then(@"the total should be ""(.*)""")]
    [Then(@"the total amount should be ""(.*)"" including tax")]
    public async Task ThenTheTotalAmountShouldBeIncludingTax(string expectedTotal)
    {
            var actualTotal = await _checkoutPage.GetTotalAmount();
            Assert.That(actualTotal, Is.EqualTo(expectedTotal), 
                $"Expected total {expectedTotal} but got {actualTotal}");
    }

    [Then(@"I should be returned to the inventory page")]
    public async Task ThenIShouldBeReturnedToTheInventoryPage()
    {
        await _checkoutPage.Page.WaitForURLAsync("**/inventory.html");
    }

    [Then(@"my cart should still contain (.*) items")]
    public async Task ThenMyCartShouldStillContainItems(int expectedCount)
    {
        var actualCount = await _cartPage.GetCartItemCount();
        Assert.That(actualCount, Is.EqualTo(expectedCount), 
            $"Expected {expectedCount} items in cart but found {actualCount}");
    }
}