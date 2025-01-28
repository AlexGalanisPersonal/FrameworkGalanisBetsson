using Microsoft.Playwright;

namespace SauceDemoUiBetsson.Pages;

public sealed class CartPage(IPage page) : BasePage(page)
{
    private readonly ILocator _cartBadge = page.Locator(".shopping_cart_badge");
    private readonly ILocator _cartLink = page.Locator(".shopping_cart_link");

    public async Task AddItemToCart(string itemName)
    {
        var itemContainer = Page.Locator(".inventory_item").Filter(new() { HasText = itemName });
        var addButton = itemContainer.Locator("button", new() { HasText = "Add to cart" });
        await addButton.ClickAsync();
    }

    public async Task RemoveItemFromCart(string itemName)
    {
        var itemContainer = Page.Locator(".inventory_item").Filter(new() { HasText = itemName });
        var removeButton = itemContainer.Locator("button", new() { HasText = "Remove" });
        await removeButton.ClickAsync();
    }
    
    public async Task<decimal> GetItemPrice(string itemName)
    {
        var itemContainer = Page.Locator(".inventory_item").Filter(new() { HasText = itemName });
        var priceText = await itemContainer.Locator(".inventory_item_price").TextContentAsync();
        return decimal.Parse(priceText?.Replace("$", "") ?? "0");
    }

    public async Task<int> GetCartItemCount()
    {
        try
        {
            var badge = await _cartBadge.TextContentAsync();
            return int.Parse(badge ?? "0");
        }
        catch
        {
            return 0;
        }
    }

    public async Task<bool> IsCartEmpty()
    {
        return !await _cartBadge.IsVisibleAsync();
    }

    public async Task OpenCart()
    {
        await _cartLink.ClickAsync();
    }
}