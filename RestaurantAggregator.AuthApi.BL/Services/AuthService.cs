using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.AuthApi.BL.Managers;
using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.AuthApi.Common.Exceptions;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.AuthApi.DAL.DBContext;
using RestaurantAggregator.AuthApi.DAL.Etities;
using RestaurantAggregator.CommonFiles;
using InvalidDataException = RestaurantAggregator.AuthApi.Common.Exceptions.InvalidDataException;

namespace RestaurantAggregator.AuthApi.BL.Services;

public class AuthService: IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly AuthDBContext _context;

    public AuthService(UserManager<User> userManager, AuthDBContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<TokenPairDto> Login(LoginCredentialDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        
        if (user == null)
        {
            throw new InvalidDataException("Invalid username or password entered");
        }

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
        {
            throw new InvalidDataException("Invalid username or password entered");
        }

        if (user.Banned)
        {
            throw new NotPermissionAccountException("You cannot log in to this account because it has been blocked by the administrator");
        }

        return await GetTokenPair(user);
    }

    public async Task<TokenPairDto> RefreshToken(TokenPairDto oldTokens)
    {
        var userId = TokenManager.GetIdOldToken(oldTokens.AccessToken);
        
        if (userId == null)
        {
            throw new InvalidDataException("Invalid token entered");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null ||
            user.RefreshToken != oldTokens.RefreshToken ||
            user.RefreshTokenExpires <= DateTime.UtcNow)
        {
            throw new InvalidDataException("Invalid token entered");
        }

        if (user.Banned)
        {
            throw new NotPermissionAccountException("You cannot log in to this account because it has been blocked by the administrator");
        }

        return await GetTokenPair(user);
    }

    public async Task Logout(string userId)
    {
        var user = await _userManager.FindByEmailAsync(userId);

        if (user == null)
        {
            throw new InvalidDataException("Invalid token entered");
        }

        user.RefreshToken = null;
        user.RefreshTokenExpires = null;

        await _userManager.UpdateAsync(user);
    }

    public async Task<TokenPairDto> RegisterCustomer(RegisterCustomerCredentialDto model)
    {
        if (model.BirthDate != null && model.BirthDate >= DateTime.UtcNow)
        {
            throw new NotCorrectDataException("Invalid birthdate. Birthdate must be more than current datetime");
        }
        
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            throw new DataAlreadyUsedException("A user with this email already exists");
        }

        var user = new User()
        {
            Email = model.Email,
            UserName = model.Username,
            BirthDate = model.BirthDate,
            Gender = model.Gender,
            PhoneNumber = model.Phone,
            Customer = new Customer
            {
                Address = model.Address
            }
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            throw new NotFountElementException("Failed to register");
        }

        await _userManager.AddToRoleAsync(user, UserRoles.Customer);

        return await GetTokenPair(user);
    }

    public async Task<TokenPairDto> RegisterWorkerAsCustomer(LoginCredentialDto model, string address)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        
        if (user == null)
        {
            throw new InvalidDataException("Invalid username or password entered");
        }

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
        {
            throw new InvalidDataException("Invalid username or password entered");
        }

        var exist = await _context.Customers.AnyAsync(c => c.Id == user.Id);
        if (exist)
        {
            throw new DataAlreadyUsedException("You are already registered");
        }

        user.Customer = new Customer
        {
            Address = address
        };

        await _userManager.UpdateAsync(user);

        await _userManager.AddToRoleAsync(user, UserRoles.Customer);

        return await GetTokenPair(user);
    }
    
    private async Task<TokenPairDto> GetTokenPair(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var accessToken = TokenManager.CreateAccessToken(claims);
        var refreshToken = TokenManager.CreateRefreshToken(claims);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = DateTime.UtcNow.AddDays(JwtConfigs.RefreshTime);

        await _userManager.UpdateAsync(user);

        return new TokenPairDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}