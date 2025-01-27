using Microsoft.Playwright;

namespace SauceDemoUiBetsson.Pages;

public sealed class CartPage(IPage page) : BasePage(page)
{
    private const string CartBadge = ".shopping_cart_badge";
    private const string CartLink = ".shopping_cart_link";

    public async Task WaitForPageLoad()
    {
        await Page.WaitForSelectorAsync(CartLink);
    }
    
    public async Task AddItemToCart(string itemName)
    {
        await WaitAndClick($"//div[text()='{itemName}']/ancestor::div[@class='inventory_item']//button[text()='Add to cart']");
    }

    public async Task RemoveItemFromCart(string itemName)
    {
        await WaitAndClick($"//div[text()='{itemName}']/ancestor::div[@class='inventory_item']//button[text()='Remove']");
    }
    
    public async Task<int> GetCartItemCount()
    {
        var badge = await Page.QuerySelectorAsync(CartBadge);
        if (badge == null) return 0;
        var count = await badge.TextContentAsync();
        return int.Parse(count ?? "0");
    }

    public async Task<bool> IsCartEmpty()
    {
        var badge = await Page.QuerySelectorAsync(CartBadge);
        return badge == null;
    }

    public async Task OpenCart()
    {
        await WaitAndClick(CartLink);
    }
}
