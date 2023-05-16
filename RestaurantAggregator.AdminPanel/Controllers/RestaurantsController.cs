using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.AdminPanel.Models;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.APIAuth.Models;
using RestaurantAggregator.AuthApi.Common.Exceptions;

namespace RestaurantAggregator.AdminPanel.Controllers;

public class RestaurantsController: Controller
{
    private readonly IRestaurantCrudService _restaurantCrudService;

    public RestaurantsController(IRestaurantCrudService restaurantCrudService)
    {
        _restaurantCrudService = restaurantCrudService;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
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
    
    [HttpGet]
    public async Task<ActionResult> Edit(Guid id)
    {
        try
        {
            var restaurant = await _restaurantCrudService.Get(id);

            return View(restaurant);
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
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Name")] UpdateInfoRestaurant
        restaurant)
    {
        if (!ModelState.IsValid)
        {
            var errorModel = new ErrorViewModel
            {
                RequestId = "Error"
            };
            
            return View("Error", errorModel);
        }

        try
        {
            await _restaurantCrudService.Update(id, new RestaurantDTO
            {
                Name = restaurant.Name
            });
            return RedirectToAction("Get");
        }
        catch (Exception e)
        {
            var errorModel = new ErrorViewModel
            {
                RequestId = "Error"
            };
            
            return View("Error", errorModel);
        }
    }

    [HttpGet]
    public async Task<ActionResult> Details(Guid id)
    {
        try
        {
            var restaurant = await _restaurantCrudService.Get(id);

            return View(restaurant);
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