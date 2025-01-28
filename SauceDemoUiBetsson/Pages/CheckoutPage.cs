using Microsoft.Playwright;
using SauceDemoUiBetsson.Models;

namespace SauceDemoUiBetsson.Pages;

public class CheckoutPage(IPage page) : BasePage(page)
{
    // Selectors
    private readonly ILocator _firstNameInput = page.Locator("[data-test='firstName']");
    private readonly ILocator _lastNameInput = page.Locator("[data-test='lastName']");
    private readonly ILocator _postalCodeInput = page.Locator("[data-test='postalCode']");
    private readonly ILocator _continueButton = page.Locator("[data-test='continue']");
    private readonly ILocator _cancelButton = page.Locator("[data-test='cancel']");
    private readonly ILocator _finishButton = page.Locator("[data-test='finish']");
    private readonly ILocator _errorMessage = page.Locator("[data-test='error']");
    private readonly ILocator _checkoutButton = page.Locator("[data-test='checkout']");
    private readonly ILocator _itemTotal = page.Locator(".summary_subtotal_label");
    private readonly ILocator _taxAmount = page.Locator(".summary_tax_label");
    private readonly ILocator _totalAmount = page.Locator(".summary_total_label");
    private readonly ILocator _successMessage = page.Locator(".complete-header");
    private readonly ILocator _cartItem = page.Locator(".cart_item");
    private readonly ILocator _inventoryItemName = page.Locator(".inventory_item_name");
    private readonly ILocator _inventoryItemPrice = page.Locator(".inventory_item_price");
    private readonly ILocator _cartQuantity = page.Locator(".cart_quantity");
    private readonly ILocator _completeContainer = page.Locator(".checkout_complete_container");
    private readonly ILocator _checkoutOverviewContainer = page.Locator(".checkout_summary_container");
    
    public async Task StartCheckout()
    {
        await _checkoutButton.ClickAsync();
        await _firstNameInput.WaitForAsync();
    }

    public async Task ClearAllFields()
    {
        await _firstNameInput.FillAsync("");
        await _lastNameInput.FillAsync("");
        await _postalCodeInput.FillAsync("");
    }

    public async Task EnterCustomerDetails(string firstName, string lastName, string postalCode)
    {
        if (!string.IsNullOrEmpty(firstName))
            await _firstNameInput.FillAsync(firstName);

        if (!string.IsNullOrEmpty(lastName))
            await _lastNameInput.FillAsync(lastName);

        if (!string.IsNullOrEmpty(postalCode))
            await _postalCodeInput.FillAsync(postalCode);
    }

    public async Task ContinueCheckout()
    {
        await _continueButton.ClickAsync();
    }


    public async Task FinishCheckout()
    {
        await _finishButton.ClickAsync();
        await _successMessage.WaitForAsync();
    }

    public async Task CancelCheckout()
    {
        await _cancelButton.ClickAsync();
    }

    public async Task<string> GetErrorMessage()
    {
        if (await _errorMessage.CountAsync() == 0)
            return string.Empty;

        return await _errorMessage.First.TextContentAsync() ?? string.Empty;
    }
    
    public async Task<List<CartItemDetails>> GetCartItemDetails()
    {
        await _checkoutOverviewContainer.WaitForAsync();
        var items = await _cartItem.AllAsync();
        var details = new List<CartItemDetails>();

        foreach (var item in items)
        {
            try
            {
                details.Add(new CartItemDetails
                {
                    Name = await item.Locator(_inventoryItemName).First.TextContentAsync() ?? string.Empty,
                    Price = await item.Locator(_inventoryItemPrice).First.TextContentAsync() ?? string.Empty,
                    Quantity = await item.Locator(_cartQuantity).First.TextContentAsync() ?? string.Empty
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing cart item: {ex.Message}");
            }
        }
        return details;
    }

    public async Task<string?> GetSubtotal()
    {
        await _checkoutOverviewContainer.WaitForAsync();
        var taxText = await _itemTotal.TextContentAsync();
        return taxText?.Split(':')[1].Trim();
    }

    public async Task<string?> GetTaxAmount()
    {
        await _checkoutOverviewContainer.WaitForAsync();
        var taxText = await _taxAmount.TextContentAsync();
        return taxText?.Split(':')[1].Trim();
    }

    public async Task<string?> GetTotalAmount()
    {
        await _checkoutOverviewContainer.WaitForAsync(new LocatorWaitForOptions 
        { 
            State = WaitForSelectorState.Visible,
            Timeout = 5000 
        });

        await _totalAmount.WaitForAsync(new LocatorWaitForOptions 
        { 
            State = WaitForSelectorState.Visible,
            Timeout = 5000 
        });

        var totalText = await _totalAmount.TextContentAsync();
        return totalText?.Split(':')[1].Trim();
    }

    public async Task<string> GetSuccessMessage()
    {
        await _completeContainer.WaitForAsync();
        return await _successMessage.First.TextContentAsync() ?? string.Empty;
    }

    public async Task FillField(string fieldName, string value)
    {
        switch (fieldName.ToLowerInvariant())
        {
            case "first name":
                await _firstNameInput.FillAsync(value);
                break;
            case "last name":
                await _lastNameInput.FillAsync(value);
                break;
            case "postal code":
            case "zip code":
                await _postalCodeInput.FillAsync(value);
                break;
            default:
                throw new ArgumentException($"Unknown field: {fieldName}");
        }
    }

    public async Task<bool> IsOnOverviewPage()
    {
        try
        {
            await _checkoutOverviewContainer.WaitForAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}