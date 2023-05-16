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

    [HttpGet]
    public async Task<ActionResult<CustomerProfileDto>> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (userId == null)
        {
            return StatusCode(403, new ResponseModel
            {
                Status = "403 error",
                Message = ""
            });
        }

        var profile = await _profileService.GetCustomerProfile(userId);

        return Ok(profile);
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeInfoProfile(ChangeIfoCustomerProfileModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirstValue((ClaimTypes.NameIdentifier));
        
        if (userId == null)
        {
            return StatusCode(403, new ResponseModel
            {
                Status = "403 error",
                Message = ""
            });
        }

        await _profileService.ChangeInfoCustomerProfile(userId, new ChangeInfoCustomerProfileDto
        {
            Username = model.Username,
            Email = model.Email,
            BirthDate = model.BirthDate,
            Gender = model.Gender,
            Phone = model.Phone,
            Address = model.Address
        });

        return Ok();
    }

    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = User.FindFirstValue((ClaimTypes.NameIdentifier));
        
        if (userId == null)
        {
            return StatusCode(403, new ResponseModel
            {
                Status = "403 error",
                Message = ""
            });
        }

        await _profileService.ChangePassword(userId, new ChangePasswordDto
        {
            OldPassword = model.OldPassword,
            NewPassword = model.NewPassword
        });

        return Ok();
    }
}