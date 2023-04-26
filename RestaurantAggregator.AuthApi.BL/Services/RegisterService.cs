using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.AuthApi.BL.Managers;
using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.AuthApi.DAL.DBContext;
using RestaurantAggregator.AuthApi.DAL.Etities;
using RestaurantAggregator.CommonFiles;

namespace RestaurantAggregator.AuthApi.BL.Services;

public class RegisterService: IRegisterService
{
    private readonly UserManager<User> _userManager;
    private readonly AuthDBContext _context;
    
    public RegisterService(UserManager<User> userManager, AuthDBContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<TokenPairDto> RegisterCustomer(RegisterCustomerCredentialDto model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            //пользователь с таким email уже есть
            throw new Exception();
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
            //не удалось зарегистрировать
        }

        await _userManager.AddToRoleAsync(user, UserRoles.Customer);

        return await GetTokenPair(user);
    }

    public async Task<TokenPairDto> RegisterWorkerAsCustomer(LoginCredentialDto model, string address)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        
        if (user == null)
        {
            //неверный логин или пароль
        }

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
        {
            //неверный логин или пароль
        }

        var exist = await _context.Customers.AnyAsync(c => c.Id == user.Id);
        if (exist)
        {
            //уже зарегистрированы
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
        user.RefreshTokenExpires = DateTime.Now.AddDays(JwtConfigs.RefreshTime);

        await _userManager.UpdateAsync(user);

        return new TokenPairDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}