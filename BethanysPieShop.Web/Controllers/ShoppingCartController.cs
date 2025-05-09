using Microsoft.AspNetCore.Mvc;
using BethanysPieShop.Web.Models;
using BethanysPieShop.Web.ViewModels;

namespace BethanysPieShop.Web.Controllers;

public class ShoppingCartController(IPieRepository pieRepo, IShoppingCart cart) : Controller
{
    public ViewResult Index()
    {
        var items = cart.GetShoppingCartItems();
        cart.ShoppingCartItems = items;
        var shoppingCartViewModel = new ShoppingCartViewModel(cart, cart.GetShoppingCartTotal());
        return View(shoppingCartViewModel);
    }

    public RedirectToActionResult AddToShoppingCart(int pieId)
    {
        var selectedPie = pieRepo.AllPies.FirstOrDefault(p => p.PieId == pieId);
        if (selectedPie != null)
            cart.AddToCart(selectedPie);
        return RedirectToAction("Index");
    }

    public RedirectToActionResult RemoveFromShoppingCart(int pieId)
    {
        var selectedPie = pieRepo.AllPies.FirstOrDefault(p => p.PieId == pieId);
        if (selectedPie != null)
            cart.RemoveFromCart(selectedPie);
        return RedirectToAction("Index");
    }
}