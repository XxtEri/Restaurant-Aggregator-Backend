using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAggregator.APIAuth.Models;
using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.AuthApi.DAL.DBContext;

namespace RestaurantAggregator.APIAuth.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticateController : ControllerBase
{
    private readonly IAuthorizeServise _authorizeService;
    private readonly IRegisterService _registerService;
    
    public AuthenticateController(IAuthorizeServise authorizeService, IRegisterService registerService)
    {
        _authorizeService = authorizeService;
        _registerService = registerService;
    }
    
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(TokenPairModel), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TokenPairModel>> RegisterCustomer([FromBody] RegisterCustomerCredentialModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var newTokenPair = await _registerService.RegisterCustomer(new RegisterCustomerCredentialDto
            {
                Username = model.Username,
                Email = model.Email,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                Phone = model.Phone,
                Password = model.Password,
                Address = model.Address
            });
            
            return Ok(new TokenPairModel
            {
                AccessToken = newTokenPair.AccessToken,
                RefreshToken = newTokenPair.RefreshToken
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPost]
    [Route("register/worker-as-customer")]
    [ProducesResponseType(typeof(TokenPairModel), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TokenPairModel>> RegisterWorkerAsCustomer([FromBody] LoginCredentialModel model, String address)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var newTokenPair = await _registerService.RegisterWorkerAsCustomer(new LoginCredentialDto
            {
                Email = model.Email,
                Password = model.Password

            }, address);
            
            return Ok(new TokenPairModel
            {
                AccessToken = newTokenPair.AccessToken,
                RefreshToken = newTokenPair.RefreshToken
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(TokenPairModel), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TokenPairModel>> Login([FromBody] LoginCredentialModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var tokenPair = await _authorizeService.Login(new LoginCredentialDto
            {
                Email = model.Email,
                Password = model.Password
            });
            
            return Ok(new TokenPairModel
            {
                AccessToken = tokenPair.AccessToken,
                RefreshToken = tokenPair.RefreshToken
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    [Route("refresh")]
    [ProducesResponseType(typeof(TokenPairModel), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TokenPairModel>> Refresh([FromBody] TokenPairModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var newTokenPair = await _authorizeService.Refresh(new TokenPairDto
            {
                AccessToken = model.AccessToken,
                RefreshToken = model.RefreshToken
            });
            
            return Ok(new TokenPairModel
            {
                AccessToken = newTokenPair.AccessToken,
                RefreshToken = newTokenPair.RefreshToken
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPost]
    [Authorize]
    [Route("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TokenPairModel>> Logout()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            //пользователь не зарегистрирован
        }

        try
        {
            await _authorizeService.Logout(userId);
            
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}