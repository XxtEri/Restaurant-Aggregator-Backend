using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.AdminPanel.Models;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.DAL.Entities;
using RestaurantAggregator.CommonFiles;

namespace RestaurantAggregator.AdminPanel.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAdminRestaurantsService _adminRestaurantsService;
    
    public HomeController(ILogger<HomeController> logger, IAdminRestaurantsService adminRestaurantsService)
    {
        _logger = logger;
        _adminRestaurantsService = adminRestaurantsService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetRestaurants()
    {
        return RedirectToAction("Get", "Restaurants");
    }
    
    public async Task<IActionResult> GetUsers()
    {
        return RedirectToAction("Get", "Users");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}