using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.AdminPanel.Models;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.APIAuth.Models;
using RestaurantAggregator.AuthApi.Common.Exceptions;
using RestaurantAggregator.CommonFiles;
using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.AdminPanel.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class RestaurantsController: Controller
{
    private readonly IAdminRestaurantsService _adminRestaurantsService;

    public RestaurantsController(IAdminRestaurantsService adminRestaurantsService)
    {
        _adminRestaurantsService = adminRestaurantsService;
    }

    [HttpGet]
    public async Task<ActionResult> Get(string? name, int page = 1)
    {
        try
        {
            var restaurantsPagedListDto = await _adminRestaurantsService.Select(name, page);
            var pageViewModel = new PageViewModel(
                restaurantsPagedListDto.PageInfoModel.Count, 
                page, 
                restaurantsPagedListDto.PageInfoModel.Size);
            
            var viewModel = new RestaurantListViewModel(
                restaurantsPagedListDto.Restaurants, 
                pageViewModel,
                name);
            
            return View(viewModel);
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
            var restaurant = await _adminRestaurantsService.Get(id);

            return View(new UpdateInfoRestaurantModel
            {
                RestaurantId = restaurant.Id,
                Name = restaurant.Name
            });
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
    public async Task<IActionResult> Edit(Guid id, UpdateInfoRestaurantModel
        restaurantModel)
    {
        if (!ModelState.IsValid)
        {
            return View(restaurantModel);
        }

        try
        {
            await _adminRestaurantsService.Update(id, new RestaurantDTO
            {
                Name = restaurantModel.Name
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
    
    public async Task<ActionResult> Details(Guid id)
    {
        try
        {
            var restaurant = await _adminRestaurantsService.Get(id);

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
            await _adminRestaurantsService.Delete(id);

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
    public async Task<IActionResult> Add()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(CreateRestaurantModel restaurantModel)
    {
        if (!ModelState.IsValid)
        {
            return View(restaurantModel);
        }

        try
        {
            await _adminRestaurantsService.Create(new CreateRestaurantDto
            {
                Name = restaurantModel.Name
            });
            
            return RedirectToAction("Get");
        }
        catch (Exception e)
        {
            var errorModel = new ErrorViewModel
            {
                RequestId = e.InnerException.Message
            };
            
            return View("Error", errorModel);
        }
    }
    
    [HttpPost]
    public string Searching(string searchString, bool notUsed)
    {
        return "From [HttpPost]Index: filter on " + searchString;
    }
}