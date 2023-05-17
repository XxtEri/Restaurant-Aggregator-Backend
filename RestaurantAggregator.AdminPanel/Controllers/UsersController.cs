using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.AdminPanel.Models;
using RestaurantAggregator.CommonFiles.Dto;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.AdminPanel.Controllers;

public class UsersController: Controller
{
    private readonly IAdminUsersServices _adminUsersServices;

    public UsersController(IAdminUsersServices adminUsersServices)
    {
        _adminUsersServices = adminUsersServices;
    }
    
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        try
        {
            var users = await _adminUsersServices.Select();
            return View(users);
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
    public Task<IActionResult> Add()
    {
        return Task.FromResult<IActionResult>(View());
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(RegisterUserCredentialModel model)
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
            await _adminUsersServices.Create(new RegisterUserCredentialDto
            {
                Username = model.Username,
                Email = model.Email,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                Phone = model.Phone,
                Password = model.Password
            });
                            
            return View("Get");
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
    
    public async Task<ActionResult<UserDto>> Details(Guid id)
    {
        try
        {
            var user = await _adminUsersServices.Get(id);

            return View(user);
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

    public async Task<IActionResult> ChangeStatusBannedUser(Guid id)
    {
        try
        {
            await _adminUsersServices.ChangeStatusBannedUser(id);

            return View("Get");
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

    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _adminUsersServices.Delete(id);

            return View("Get");
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
            var restaurant = await _adminUsersServices.Get(id);

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