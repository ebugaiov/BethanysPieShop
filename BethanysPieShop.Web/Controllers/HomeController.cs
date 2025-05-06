using Microsoft.AspNetCore.Mvc;
using BethanysPieShop.Web.Models;
using BethanysPieShop.Web.ViewModels;

namespace BethanysPieShop.Web.Controllers;

public class HomeController : Controller
{
    private readonly IPieRepository _pieRepository;

    public HomeController(IPieRepository pieRepository)
    {
        _pieRepository = pieRepository;
    }

    public ViewResult Index()
    {
        var piesOfTheWeek = _pieRepository.PiesOfTheWeek;
        var homeViewModel = new HomeViewModel(piesOfTheWeek);
        return View(homeViewModel);
    }
}