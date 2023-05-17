using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.AuthApi.Common.Exceptions;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.AuthApi.DAL.DBContext;
using RestaurantAggregator.AuthApi.DAL.Etities;
using RestaurantAggregator.CommonFiles.Dto;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.AuthApi.BL.Services;

public class AdminProfileService: IAdminProfileService
{
    private readonly AuthDBContext _context;

    public AdminProfileService(AuthDBContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> GetUser()
    {
        var users = await _context.Users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.UserName,
            Email = u.Email,
            BirthDate = u.BirthDate,
            Gender = u.Gender,
            Phone = u.PhoneNumber
        }).ToListAsync();

        if (users == null)
        {
            throw new NotFoundElementException("Не найдено ни одного пользователя");
        }
        
        foreach (var user in users)
        {
            var roleId = await _context.UserRoles
                .Where(o => o.UserId == user.Id)
                .Select(o => o.RoleId)
                .FirstOrDefaultAsync();

            user.Role = await _context.Roles
                .Where(r => r.Id == roleId)
                .Select(r => r.Name)
                .FirstOrDefaultAsync();
        }

        return users;
    }
    
    public Task GetFullUserAccount(string userId)
    {
        throw new NotImplementedException();
    }

    public Task ChangeUser(string userId, ChangeUserDTO model)
    {
        throw new NotImplementedException();
    }

    public Task SetStatusBannedUser(string userId, bool isBanned)
    {
        throw new NotImplementedException();
    }

    public Task AppointManagerInRestaurant(string managerId, Guid restaurantId)
    {
        throw new NotImplementedException();
    }

    public Task AppointCookInRestaurant(string cookId, Guid restaurantId)
    {
        throw new NotImplementedException();
    }

    public Task RegisterUser(RegisterUserCredentialDto model)
    {
        throw new NotImplementedException();
    }

    public Task RegisterManager(string userId, Guid restaurantId)
    {
        throw new NotImplementedException();
    }

    public Task RegisterCook(string userId, Guid restaurantId)
    {
        throw new NotImplementedException();
    }

    public Task RegisterCourier(string userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUser(string userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteManager(string managerId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCook(string cookId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCourier(string courierId)
    {
        throw new NotImplementedException();
    }
}