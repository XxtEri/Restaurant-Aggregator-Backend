using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.AuthApi.DAL.DBContext;
using RestaurantAggregator.AuthApi.DAL.Etities;
using RestaurantAggregator.CommonFiles.Enums;
using RestaurantAggregator.CommonFiles.Exceptions;

namespace RestaurantAggregator.AuthApi.BL.Services;

public class ProfileService: IProfileService
{
    private readonly UserManager<User> _userManager;
    private readonly AuthDBContext _context;

    public ProfileService(UserManager<User> userManager, AuthDBContext context)
    {
        _context = context;
        _userManager = userManager;
    }
    
    public async Task<CustomerProfileDto> GetCustomerProfile(Guid userId)
    {
        var customer = await _context.Customers.FindAsync(userId);

        if (customer == null)
        {
            throw new NotFoundException($"Покупатель с id = {userId} не найден");
        }

        var user = await _context.Users.FindAsync(userId);
        
        if (user == null)
        {
            throw new NotFoundException($"Пользователь с id = {userId} не найден");
        }

        return new CustomerProfileDto
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            Phone = user.PhoneNumber,
            Address = customer.Address
        };
    }
    
    public async Task ChangeInfoCustomerProfile(Guid userId, ChangeInfoCustomerProfileDto model)
    {
        var customer = await _context.Customers.FindAsync(userId);

        if (customer == null)
        {
            throw new NotFoundException($"Покупатель с id = {userId} не найден");
        }

        var user = await _context.Users.FindAsync(userId);
        
        if (user == null)
        {
            throw new NotFoundException($"Пользователь с id = {userId} не найден");
        }

        user.UserName = model.Username;
        user.BirthDate = model.BirthDate;
        user.Gender = model.Gender;
        user.PhoneNumber = model.Phone;
        customer.Address = model.Address;
        
        _context.Entry(user).State = EntityState.Modified;
        _context.Entry(customer).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    //TODO: проверить
    public async Task ChangePassword(Guid userId, ChangePasswordDto model)
    {
        var customer = await _context.Customers.FindAsync(userId);

        if (customer == null)
        {
            throw new NotFoundException($"Покупатель с id = {userId} не найден");
        }

        var user = await _context.Users.FindAsync(userId);
        
        if (user == null)
        {
            throw new NotFoundException($"Пользователь с id = {userId} не найден");
        }
        
        var isOldPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.OldPassword);

        if (!isOldPasswordCorrect)
        {
            throw new NotCorrectDataException("Указан неверный пароль");
        }
        
        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                
        if (!result.Succeeded)
        {
            throw new InvalidResponseException("Что-то пошло не так при изменении пароля");
        }
    }

    public string? GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
        var userId = jwtToken?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        return userId;
    }
}