using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.AdminPanel.Models;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.DAL.Entities;

namespace RestaurantAggregator.AdminPanel.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRestaurantCrudService _restaurantCrudService;
    
    public HomeController(ILogger<HomeController> logger, IRestaurantCrudService restaurantCrudService)
    {
        _logger = logger;
        _restaurantCrudService = restaurantCrudService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> GetRestaurants()
    {
        return RedirectToAction("Get", "Restaurants");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}