using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.AuthApi.Common.Exceptions;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.AuthApi.DAL.DBContext;
using RestaurantAggregator.AuthApi.DAL.Etities;
using RestaurantAggregator.CommonFiles;
using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.AuthApi.BL.Services;

public class AdminProfileService: IAdminProfileService
{
    private readonly UserManager<User> _userManager;
    private readonly AuthDBContext _context;

    public AdminProfileService(UserManager<User> userManager, AuthDBContext context)
    {
        _userManager = userManager;
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
                isBanned = u.Banned,
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
                isBanned = u.Banned,
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

    public async Task ChangeStatusBannedUser(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundElementException($"Не найдено пользователя с id = {userId}");
        }
        
        user.Banned = user.Banned == false;
        
        _context.Users.Attach(user);
        _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }
    
    public Task ChangeUser(Guid userId, UpdateInfoUserProfileDto model)
    {
        throw new NotImplementedException();
    }

    public async Task AppointManagerInRestaurant(Guid managerId, Guid restaurantId)
    {
        //типа проверили заранее restaurantId есть или нет такой

        var manager = await _context.Managers
            .Where(m => m.Id == managerId)
            .FirstOrDefaultAsync();

        if (manager == null)
        {
            throw new NotFoundElementException($"Не найден менеджер с id = {managerId}");
        }

        manager.RestaurantId = restaurantId;

        _context.Managers.Attach(manager);
        _context.Entry(manager).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task AppointCookInRestaurant(Guid cookId, Guid restaurantId)
    {
        //типа проверили заранее restaurantId есть или нет такой

        var cook = await _context.Cooks
            .Where(m => m.Id == cookId)
            .FirstOrDefaultAsync();

        if (cook == null)
        {
            throw new NotFoundElementException($"Не найден менеджер с id = {cookId}");
        }

        cook.RestaurantId = restaurantId;

        _context.Cooks.Attach(cook);
        _context.Entry(cook).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task RegisterUser(RegisterUserCredentialDto model)
    {
        if (model.BirthDate >= DateTime.UtcNow)
        {
            throw new NotCorrectDataException("Invalid birthdate. Birthdate must be more than current datetime");
        }

        model.BirthDate = model.BirthDate.ToUniversalTime();

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            throw new DataAlreadyUsedException("A user with this email already exists");
        }

        var user = new User
        {
            Email = model.Email,
            UserName = model.Username,
            BirthDate = model.BirthDate,
            Gender = model.Gender,
            PhoneNumber = model.Phone
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            throw new NotFoundElementException("Failed to register");
        }
    }

    public async Task RegisterManager(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundElementException($"Пользователь с id = {userId} не найден");
        }
        
        var manager = new Manager
        {
            Id = userId,
            User = user
        };

        await _context.Managers.AddAsync(manager);
        await _context.SaveChangesAsync();
    }

    public async Task RegisterCook(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundElementException($"Пользователь с id = {userId} не найден");
        }
        
        var cook = new Cook
        {
            Id = userId,
            User = user
        };

        await _context.Cooks.AddAsync(cook);
        await _context.SaveChangesAsync();
    }

    public async Task RegisterCourier(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundElementException($"Пользователь с id = {userId} не найден");
        }
        
        var courier = new Courier
        {
            Id = userId,
            User = user
        };

        await _context.Couriers.AddAsync(courier);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundElementException($"Не удалось найти пользователя с id = {userId}");
        }

        var roles = await GetRolesForUser(user.Id);

        foreach (var role in roles)
        {
            switch (role)
            {
                case UserRoles.Customer:
                    await DeleteCustomer(user.Id);
                    break;
                case UserRoles.Manager:
                    await DeleteManager(user.Id);
                    break;
                case UserRoles.Cook:
                    await DeleteCook(user.Id);
                    break;
                case UserRoles.Courier:
                    await DeleteCourier(user.Id);
                    break;
            }
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    private async Task DeleteManager(Guid managerId)
    {
        var manager = await _context.Managers
            .Where(m => m.Id == managerId)
            .FirstOrDefaultAsync();

        if (manager != null)
        {
            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();
        }
    }

    private async Task DeleteCook(Guid cookId)
    {
        var cook = await _context.Cooks
            .Where(c => c.Id == cookId)
            .FirstOrDefaultAsync();

        if (cook != null)
        {
            _context.Cooks.Remove(cook);
            await _context.SaveChangesAsync();
        }
    }

    private async Task DeleteCourier(Guid courierId)
    {
        var courier = await _context.Couriers
            .Where(c => c.Id == courierId)
            .FirstOrDefaultAsync();

        if (courier != null)
        {
            _context.Couriers.Remove(courier);
            await _context.SaveChangesAsync();
        }
    }

    private async Task DeleteCustomer(Guid customerId)
    {
        var customer = await _context.Customers
            .Where(c => c.Id == customerId)
            .FirstOrDefaultAsync();

        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }

    private async Task<List<string>> GetRolesForUser(Guid id)
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
    
}