using Microsoft.AspNetCore.Mvc;
using BethanysPieShop.Web.Models;
using BethanysPieShop.Web.ViewModels;

namespace BethanysPieShop.Web.Components;

public class ShoppingCartSummary(IShoppingCart cart) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var items = cart.GetShoppingCartItems();
        cart.ShoppingCartItems = items;

        var shoppingCartViewModel = new ShoppingCartViewModel(cart, cart.GetShoppingCartTotal());
        
        return View(shoppingCartViewModel);
    }
}