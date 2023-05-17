using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.AdminPanel.Models;
using RestaurantAggregator.AuthApi.Common.IServices;

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
            var users = await _adminUsersServices.Get();
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
}