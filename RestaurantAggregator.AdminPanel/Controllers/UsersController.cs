using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.AdminPanel.Models;
using RestaurantAggregator.CommonFiles;
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
    public async Task<IActionResult> Add()
    {
        return View();
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
    public async Task<ActionResult<UserDto>> Edit(Guid id)
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
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateInfoUserProfileModel model)
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
            await _adminUsersServices.ChangeInfoUserProfile(id, new UpdateInfoUserProfileDto
            {
                Username = model.Username,
                Email = model.Email,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                Phone = model.Phone,
                Address = model.Address
            });
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
    
    public async Task<IActionResult> AddManagerRole(Guid id)
    {
        try
        {
            await _adminUsersServices.AddManagerRole(id);

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
    
    public async Task<IActionResult> AddCookRole(Guid id)
    {
        try
        {
            await _adminUsersServices.AddCookRole(id);

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
    
    public async Task<IActionResult> AddCourierRole(Guid id)
    {
        try
        {
            await _adminUsersServices.AddCourierRole(id);

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

    public async Task<IActionResult> DeleteManagerRole(Guid id)
    {
        try
        {
            await _adminUsersServices.DeleteManagerRole(id);

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
    
    public async Task<IActionResult> DeleteCookRole(Guid id)
    {
        try
        {
            await _adminUsersServices.DeleteCookRole(id);

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
    
    public async Task<IActionResult> DeleteCourierRole(Guid id)
    {
        try
        {
            await _adminUsersServices.DeleteCourierRole(id);

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
    public async Task<IActionResult> EditRestaurantIdForManager(Guid id)
    {
        try
        {
            var restaurantId = await _adminUsersServices.GetRestaurantIdForManager(id);
            var model = new ChangeRestaurantIdViewModel
            {
                UserId = id,
                RestaurantId = restaurantId
            };
            
            return View(model);
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
    public async Task<ActionResult<UserDto>> EditRestaurantIdForManager(Guid id, ChangeRestaurantIdModel model)
    {
        try
        {
            var user = await _adminUsersServices.Get(id);
            
            await _adminUsersServices.AppointManagerInRestaurant(id, model.RestaurantId);

            return RedirectToAction("Details", user);
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
    public async Task<IActionResult> EditRestaurantIdForCook(Guid id)
    {
        try
        {
            var restaurantId = await _adminUsersServices.GetRestaurantIdForManager(id);
            var model = new ChangeRestaurantIdViewModel
            {
                UserId = id,
                RestaurantId = restaurantId
            };
            
            return View(model);
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
    public async Task<ActionResult<UserDto>> EditRestaurantIdForCook(Guid id, ChangeRestaurantIdModel model)
    {
        try
        {
            var user = await _adminUsersServices.Get(id);
            
            await _adminUsersServices.AppointCookInRestaurant(id, model.RestaurantId);

            return RedirectToAction("Details", user);
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