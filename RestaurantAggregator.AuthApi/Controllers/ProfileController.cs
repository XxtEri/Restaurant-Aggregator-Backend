using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.APIAuth.Models;
using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.CommonFiles;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.APIAuth.Controllers;

[Route("profile")]
[ApiController]
[Authorize(Roles = UserRoles.Customer)]
public class ProfileController: ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    /// <summary>
    /// Получение информации профиля пользователя с ролью Customer
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = UserRoles.Customer)]
    public async Task<ActionResult<CustomerProfileDto>> GetProfile()
    {
        var userId = Guid.Parse(User.Identity!.Name!);

        var profile = await _profileService.GetCustomerProfile(userId);

        return Ok(profile);
    }

    /// <summary>
    /// Изменения информации профиля пользователя с ролью Customer
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch]
    [Authorize(Roles = UserRoles.Customer)]
    public async Task<IActionResult> ChangeInfoProfile(ChangeIfoCustomerProfileModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = Guid.Parse(User.Identity!.Name!);

        await _profileService.ChangeInfoCustomerProfile(userId, new ChangeInfoCustomerProfileDto
        {
            Username = model.Username,
            BirthDate = model.BirthDate,
            Gender = model.Gender,
            Phone = model.Phone,
            Address = model.Address
        });

        return Ok();
    }

    /// <summary>
    /// Изменение пароля аккаунта пользователя с ролью Customer
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("password")]
    [Authorize(Roles = UserRoles.Customer)]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = Guid.Parse(User.Identity!.Name!);

        await _profileService.ChangePassword(userId, new ChangePasswordDto
        {
            OldPassword = model.OldPassword,
            NewPassword = model.NewPassword
        });

        return Ok();
    }
}