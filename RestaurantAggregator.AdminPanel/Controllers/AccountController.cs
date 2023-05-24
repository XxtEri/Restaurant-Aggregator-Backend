using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.AdminPanel.Common.Dto;
using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.AdminPanel.Models;

namespace RestaurantAggregator.AdminPanel.Controllers;

public class LoginController: Controller
{
    private readonly IAuthService _authService;

    public LoginController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginCredentialModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var claimsIdentity = await _authService.Login(new LoginCredentialDto
        {
            Email = model.Email,
            Password = model.Password
        });
        
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        
        return RedirectToAction("Index", "Home");
    }
}