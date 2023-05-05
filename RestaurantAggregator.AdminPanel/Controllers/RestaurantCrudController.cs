using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.AdminPanel.Models;
using RestaurantAggregator.AuthApi.Common.Exceptions;

namespace RestaurantAggregator.AdminPanel.Controllers;

public class RestaurantCrudController: Controller
{
    private readonly IRestaurantCrudService _restaurantCrudService;

    public RestaurantCrudController(IRestaurantCrudService restaurantCrudService)
    {
        _restaurantCrudService = restaurantCrudService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRestaurants()
    {
        try
        {
            var restaurants = await _restaurantCrudService.Select();
            return View(restaurants);
        }
        catch (Exception e)
        {
            var errorModel = new ErrorViewModel
            {
                RequestId = e.Message
            };

            return View("Error", errorModel);
        }
    }
}