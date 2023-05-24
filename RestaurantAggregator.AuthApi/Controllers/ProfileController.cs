using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _profileService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }

        var profile = await _profileService.GetCustomerProfile(new Guid(userId));

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

        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _profileService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }

        var profile = await _profileService.GetCustomerProfile(new Guid(userId));

        await _profileService.ChangeInfoCustomerProfile(new Guid(userId), new ChangeInfoCustomerProfileDto
        {
            Username = profile.Username,
            BirthDate = profile.BirthDate,
            Gender = profile.Gender,
            Phone = profile.Phone,
            Address = profile.Address
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
        
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _profileService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }

        var profile = await _profileService.GetCustomerProfile(new Guid(userId));

        await _profileService.ChangePassword(new Guid(userId), new ChangePasswordDto
        {
            OldPassword = model.OldPassword,
            NewPassword = model.NewPassword
        });

        return Ok();
    }
}