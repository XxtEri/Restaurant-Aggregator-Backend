using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.AdminPanel.Models;

namespace RestaurantAggregator.AdminPanel.Controllers;

public class LoginController: Controller
{
    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Verify(AccountModel model)
    {
        if (model.Name == "admin" && model.Password == "admin") {
            return RedirectToAction("Index", "Home");
        }

        return View("Login");
    }
}