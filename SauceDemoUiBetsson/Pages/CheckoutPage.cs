using Microsoft.Playwright;
using SauceDemoUiBetsson.Models;

namespace SauceDemoUiBetsson.Pages;

public class CheckoutPage(IPage page) : BasePage(page)
{
    // Selectors
    private const string FirstNameInput = "[data-test='firstName']";
    private const string LastNameInput = "[data-test='lastName']";
    private const string PostalCodeInput = "[data-test='postalCode']";
    private const string ContinueButton = "[data-test='continue']";
    private const string CancelButton = "[data-test='cancel']";
    private const string FinishButton = "[data-test='finish']";
    private const string ErrorMessage = "[data-test='error']";
    private const string CheckoutButton = "[data-test='checkout']";
    private const string ItemTotal = ".summary_subtotal_label";
    private const string TaxAmount = ".summary_tax_label";
    private const string TotalAmount = ".summary_total_label";
    private const string SuccessMessage = ".complete-header";
    private const string CartItem = ".cart_item";
    private const string InventoryItemName = ".inventory_item_name";
    private const string InventoryItemPrice = ".inventory_item_price";
    private const string CartQuantity = ".cart_quantity";
    private const string CompleteContainer = ".checkout_complete_container";
    private const string CheckoutOverviewContainer = ".checkout_summary_container";
    
    public async Task StartCheckout()
    {
        await WaitAndClick(CheckoutButton);
        await Page.WaitForSelectorAsync(FirstNameInput);
    }

    public async Task ClearAllFields()
    {
        await Page.FillAsync(FirstNameInput, "");
        await Page.FillAsync(LastNameInput, "");
        await Page.FillAsync(PostalCodeInput, "");
    }

    public async Task EnterCustomerDetails(string firstName, string lastName, string postalCode)
    {
        if (!string.IsNullOrEmpty(firstName))
            await WaitAndFill(FirstNameInput, firstName);

        if (!string.IsNullOrEmpty(lastName))
            await WaitAndFill(LastNameInput, lastName);

        if (!string.IsNullOrEmpty(postalCode))
            await WaitAndFill(PostalCodeInput, postalCode);
    }

    public async Task ContinueCheckout()
    {
        await WaitAndClick(ContinueButton);
    }

    public async Task FinishCheckout()
    {
        await WaitAndClick(FinishButton);
        await Page.WaitForSelectorAsync(SuccessMessage);
    }

    public async Task CancelCheckout()
    {
        await WaitAndClick(CancelButton);
    }

    public async Task<string> GetErrorMessage()
    {
        var errorElement = await Page.QuerySelectorAsync(ErrorMessage);
        return errorElement != null ? await errorElement.TextContentAsync() ?? string.Empty : string.Empty;
    }

    public async Task<List<CartItemDetails>> GetCartItemDetails()
    {
        await Page.WaitForSelectorAsync(CheckoutOverviewContainer);

        var items = await Page.QuerySelectorAllAsync(CartItem);
        var details = new List<CartItemDetails>();

        foreach (var item in items)
        {
            try
            {
                var nameElement = await item.QuerySelectorAsync(InventoryItemName);
                var priceElement = await item.QuerySelectorAsync(InventoryItemPrice);
                var quantityElement = await item.QuerySelectorAsync(CartQuantity);

                if (nameElement != null && priceElement != null && quantityElement != null)
                {
                    details.Add(new CartItemDetails
                    {
                        Name = await nameElement.TextContentAsync() ?? string.Empty,
                        Price = await priceElement.TextContentAsync() ?? string.Empty,
                        Quantity = await quantityElement.TextContentAsync() ?? string.Empty
                    });
                }
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
        await Page.WaitForSelectorAsync(CheckoutOverviewContainer);
        var taxText = await WaitAndGetText(ItemTotal);
        return taxText?.Split(':')[1].Trim();
    }

    public async Task<string?> GetTaxAmount()
    {
        await Page.WaitForSelectorAsync(CheckoutOverviewContainer);
        var taxText = await WaitAndGetText(TaxAmount);
        return taxText?.Split(':')[1].Trim();
    }

    public async Task<string?> GetTotalAmount()
    {
        await Page.WaitForSelectorAsync(CheckoutOverviewContainer, new PageWaitForSelectorOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 5000
        });

        await Page.WaitForSelectorAsync(TotalAmount, new PageWaitForSelectorOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 5000
        });

        var totalText = await WaitAndGetText(TotalAmount);
        return totalText?.Split(':')[1].Trim();
    }

    public async Task<string> GetSuccessMessage()
    {
        await Page.WaitForSelectorAsync(CompleteContainer);
        var messageElement = await Page.QuerySelectorAsync(SuccessMessage);
        return messageElement != null ? await messageElement.TextContentAsync() ?? string.Empty : string.Empty;
    }
}