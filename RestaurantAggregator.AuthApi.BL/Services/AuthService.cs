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
using RestaurantAggregator.CommonFiles.Enums;
using RestaurantAggregator.CommonFiles.Exceptions;

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

    public async Task RegisterAdmin()
    {
        const string email = "admin@gmail.com";

        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser == null)
        {
            const string username = "admin";
            const string phone = "12345678911";
            const string password = "Admin1";
            
            var user = new User()
            {
                Email = email,
                UserName = username,
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Female,
                PhoneNumber = phone
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new NotFoundElementException("Failed to register");
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        }
    }

    public async Task<ClaimsIdentity> LoginAdmin(LoginCredentialDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        
        await CheckUserAndPassword(user, model.Password);
        
        var claims = await GetClaims(user);
        
        return new ClaimsIdentity(claims, "Cookies");
    }

    public async Task<TokenPairDto> Login(LoginCredentialDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        
        await CheckUserAndPassword(user, model.Password);

        if (user.Banned)
        {
            throw new NotPermissionAccountException("You cannot log in to this account because it has been blocked by the administrator");
        }

        return await GetTokenPair(user);
    }

    public async Task<TokenPairDto> RefreshToken(TokenPairDto oldTokens)
    {
        var userId = TokenManager.GetUserId(oldTokens.AccessToken);
        
        if (userId == null)
        {
            throw new InvalidDataCustomException("Invalid token entered");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null ||
            user.RefreshToken != oldTokens.RefreshToken ||
            user.RefreshTokenExpires <= DateTime.UtcNow)
        {
            throw new InvalidDataCustomException("Invalid token entered");
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
            throw new InvalidDataCustomException("Invalid token entered");
        }

        user.RefreshToken = null;
        user.RefreshTokenExpires = null;

        await _userManager.UpdateAsync(user);
    }

    public async Task<TokenPairDto> RegisterCustomer(RegisterCustomerCredentialDto model)
    {
        if (model.BirthDate >= DateTime.UtcNow)
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
            throw new NotFoundElementException("Failed to register");
        }

        await _userManager.AddToRoleAsync(user, UserRoles.Customer);

        return await GetTokenPair(user);
    }

    public async Task<TokenPairDto> RegisterWorkerAsCustomer(LoginCredentialDto model, string address)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        
        if (user == null)
        {
            throw new InvalidDataCustomException("Invalid username or password entered");
        }

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
        {
            throw new InvalidDataCustomException("Invalid username or password entered");
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
        var claims = await GetClaims(user);

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

    private async Task CheckUserAndPassword(User user, string password)
    {
        if (user == null)
        {
            throw new InvalidDataCustomException("Invalid username or password entered");
        }

        if (!await _userManager.CheckPasswordAsync(user, password))
        {
            throw new InvalidDataCustomException("Invalid username or password entered");
        }
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
}