using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.AuthApi.Common.Exceptions;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.AuthApi.DAL.DBContext;
using RestaurantAggregator.AuthApi.DAL.Etities;
using RestaurantAggregator.CommonFiles;
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

    public async Task<List<UserDto>> GetUsers()
    {
        var users = await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email,
                BirthDate = u.BirthDate,
                Gender = u.Gender,
                Phone = u.PhoneNumber,
                Roles = new List<string>()
            })
            .ToListAsync();

        if (users == null)
        {
            throw new NotFoundElementException("Не найдено ни одного пользователя");
        }
        
        foreach (var user in users)
        {
            user.Roles = await GetRolesForUser(user.Id);
        }

        return users;
    }
    
    public async Task<UserDto> GetUser(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email,
                BirthDate = u.BirthDate,
                Gender = u.Gender,
                Phone = u.PhoneNumber,
                isCourier = false
            })
            .FirstOrDefaultAsync();
        

        if (user == null)
        {
            throw new NotFoundElementException($"Пользователь с id = {userId} не найден");
        }
        
        user.Roles = await GetRolesForUser(user.Id);

        if (user.Roles.Contains(UserRoles.Customer))
        {
            user.Address = await _context.Customers
                .Where(c => c.Id == user.Id)
                .Select(c => c.Address)
                .FirstAsync();
        }
        
        if (user.Roles.Contains(UserRoles.Manager))
        {
            user.ManagerRestaurantId = await _context.Managers
                .Where(m => m.Id == user.Id)
                .Select(m => m.RestaurantId)
                .FirstAsync();
        }
        
        if (user.Roles.Contains(UserRoles.Cook))
        {
            user.CookRestaurantId = await _context.Cooks
                .Where(m => m.Id == user.Id)
                .Select(m => m.RestaurantId)
                .FirstAsync();
        }
        
        if (user.Roles.Contains(UserRoles.Courier))
        {
            user.isCourier = true;
        }

        return user;
    }

    public async Task<List<string>> GetRolesForUser(Guid id)
    {
        var rolesId = await _context.UserRoles
            .Where(o => o.UserId == id)
            .Select(o => o.RoleId)
            .ToListAsync();

        var roleNames = new List<string>();
        
        foreach (var roleId in rolesId)
        {
            var roleName = await _context.Roles
                .Where(r => r.Id == roleId)
                .Select(r => r.Name)
                .FirstAsync();

            roleNames.Add(roleName);
        }

        return roleNames;
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