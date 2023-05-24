using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.APIAuth.Models;
using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.AuthApi.Common.IServices;

namespace RestaurantAggregator.APIAuth.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticateController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticateController(IAuthService authService)
    {
        _authService = authService;
    }
    
    /// <summary>
    /// Register customer to the system
    /// </summary>
    /// /// <param name="request">register in request</param>
    /// <returns>jwt tokens</returns>
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(TokenPairModel), StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TokenPairModel>> RegisterCustomer([FromBody] RegisterCustomerCredentialModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseModel
            {
                Status = "404",
                Message = "Model is not correct"
            });
        }
        
        var newTokenPair = await _authService.RegisterCustomer(new RegisterCustomerCredentialDto
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
    
    /// <summary>
    /// Register worker as customer to the system
    /// </summary>
    /// /// <param name="request">register in request</param>
    /// <returns>jwt tokens</returns>
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
        
        var newTokenPair = await _authService.RegisterWorkerAsCustomer(new LoginCredentialDto
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

    /// <summary>
    /// Log in to the system
    /// </summary>
    /// /// <param name="request">log in request</param>
    /// <returns>jwt tokens</returns>
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
            return BadRequest(new ResponseModel
            {
                Status = "404",
                Message = ModelState.ToString()
            });
        }
        
        var tokenPair = await _authService.Login(new LoginCredentialDto
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

    /// <summary>
    /// Get refresh token for user
    /// </summary>
    /// <returns>jwt tokens</returns>
    [HttpPost]
    [Route("refresh")]
    [Authorize]
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

        var newTokenPair = await _authService.RefreshToken(new TokenPairDto
        {
            AccessToken = model.AccessToken,
            RefreshToken = model.RefreshToken
        });

        return Ok(new TokenPairModel
            {
                AccessToken = newTokenPair.AccessToken,
                RefreshToken = newTokenPair.RefreshToken
            }
        );
    }
    
    /// <summary>
    /// Logout in the system
    /// </summary>
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
        
        await _authService.Logout(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return Ok();
    }
}