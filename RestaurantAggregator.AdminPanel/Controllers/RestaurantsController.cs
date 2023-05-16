using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.AdminPanel.Models;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.APIAuth.Models;
using RestaurantAggregator.AuthApi.Common.Exceptions;
using RestaurantAggregator.CommonFiles.Dto;

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
    public async Task<IActionResult> Edit(Guid id, [Bind("Name")] UpdateInfoRestaurantModel
        restaurantModel)
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
                Name = restaurantModel.Name
            });
            return View("Get");
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
    
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _restaurantCrudService.Delete(id);

            return RedirectToAction("Get");
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
    public async Task<IActionResult> Add(Guid id)
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(CreateRestaurantModel restaurantModel)
    {
        if (!ModelState.IsValid)
        {
            var errorModel = new ErrorViewModel
            {
                RequestId = ModelState.ToString()
            };
            
            return View("Error", errorModel);
        }

        try
        {
            await _restaurantCrudService.Create(new CreateRestaurantDto
            {
                Name = restaurantModel.Name
            });
            
            return View("Get");
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
}