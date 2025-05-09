using Microsoft.AspNetCore.Mvc;
using BethanysPieShop.Web.Models;

namespace BethanysPieShop.Web.Components;

public class CategoryMenu(ICategoryRepository catRepo) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var categories = catRepo.AllCategories.OrderBy(c => c.CategoryName);
        return View(categories);
    }
}