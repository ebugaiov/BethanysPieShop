using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Web.Models;

public class ShoppingCart(BethanysPieShopDbContext context) : IShoppingCart
{
    public string? ShoppingCartId { get; set; }
    
    public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;

    public static ShoppingCart GetShoppingCart(IServiceProvider services)
    {
        ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
        BethanysPieShopDbContext context = services.GetService<BethanysPieShopDbContext>() ?? throw new Exception("Error Initializing the ShoppingCart");
        string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
        session?.SetString("CartId", cartId);
        return new ShoppingCart(context) { ShoppingCartId = cartId };
    }

    public void AddToCart(Pie pie)
    {
        var shoppingCartItem =
            context.ShoppingCartItems.SingleOrDefault(s =>
                s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId
            );
        if (shoppingCartItem == null)
        {
            shoppingCartItem = new ShoppingCartItem
            {
                ShoppingCartId = ShoppingCartId,
                Pie = pie,
                Amount = 1
            };
            context.ShoppingCartItems.Add(shoppingCartItem);
        }
        else
        {
            shoppingCartItem.Amount++;
        }
        context.SaveChanges();
    }

    public int RemoveFromCart(Pie pie)
    {
        var shoppingCartItem = context.ShoppingCartItems.SingleOrDefault(
            s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId
            );
        var localAmount = 0;
        if (shoppingCartItem != null)
        {
            shoppingCartItem.Amount--;
            localAmount = shoppingCartItem.Amount;
        }
        else
        {
            context.ShoppingCartItems.Remove(shoppingCartItem);
        }
        context.SaveChanges();
        return localAmount;
    }

    public List<ShoppingCartItem> GetShoppingCartItems()
    {
        return ShoppingCartItems ??= context.ShoppingCartItems
            .Where(c => c.ShoppingCartId == ShoppingCartId)
            .Include(s => s.Pie)
            .ToList();
    }
    
    public void ClearCart()
    {
        var cartItems = context.ShoppingCartItems
            .Where(c => c.ShoppingCartId == ShoppingCartId);
        context.ShoppingCartItems.RemoveRange(cartItems);
        context.SaveChanges();
    }

    public decimal GetShoppingCartTotal()
    {
        var total = context.ShoppingCartItems
            .Where(c => c.ShoppingCartId == ShoppingCartId)
            .Sum(c => c.Pie.Price * c.Amount);
        return total;
    }
}