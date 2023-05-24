using System.Security.Claims;
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
using RestaurantAggregator.CommonFiles.Exceptions;

namespace RestaurantAggregator.AuthApi.BL.Services;

public class AdminProfileService: IAdminProfileService
{
    private readonly UserManager<User> _userManager;
    private readonly AuthDBContext _context;
    private readonly IAuthService _authService;

    public AdminProfileService(UserManager<User> userManager, AuthDBContext context, IAuthService authService)
    {
        _userManager = userManager;
        _context = context;
        _authService = authService;
    }

    public async Task<ClaimsIdentity> LoginAdmin(LoginCredentialDto model)
    {
        return await _authService.LoginAdmin(model);
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
    
    public async Task ChangeUser(Guid userId, UpdateInfoUserProfileDto model)
    {
        if (model.BirthDate >= DateTime.UtcNow)
        {
            throw new NotCorrectDataException("Invalid birthdate. Birthdate must be more than current datetime");
        }

        model.BirthDate = model.BirthDate.ToUniversalTime();
        
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            throw new NotFoundElementException($"Ресторан для внесения изменений с id = {userId} не найден");
        }

        user.UserName = model.Username;
        user.PhoneNumber = model.Phone;
        user.BirthDate = model.BirthDate;
        user.Gender = model.Gender;

        if (model.Address != null)
        {
            var customer = await _context.Customers
                .Where(c => c.Id == userId)
                .FirstOrDefaultAsync();
            
            if (customer != null)
            {
                customer.Address = model.Address;
                
                _context.Customers.Attach(customer);
                _context.Entry(customer).State = EntityState.Modified;
            }
        }

        _context.Users.Attach(user);
        _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task AppointManagerInRestaurant(Guid managerId, Guid restaurantId)
    {
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
        
        await _userManager.AddToRoleAsync(user, UserRoles.Manager);

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
        
        await _userManager.AddToRoleAsync(user, UserRoles.Cook);
        
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
        
        await _userManager.AddToRoleAsync(user, UserRoles.Courier);
        
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

    public async Task<Guid?> GetRestaurantIdForManager(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();
        
        if (user == null)
        {
            throw new NotFoundElementException($"Пользователь с id = {userId} не найден");
        }

        var roleIdManager = await _context.Roles
            .Where(r => r.Name == UserRoles.Manager)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        var roleManager = await _context.UserRoles
            .Where(o => o.UserId == userId && o.RoleId == roleIdManager)
            .FirstOrDefaultAsync();

        if (roleManager == null)
        {
            throw new NotFoundElementException($"Пользователь с id = {userId} не имеет роль менеджера");
        }

        var manager = await _context.Managers
            .Where(m => m.Id == userId)
            .FirstOrDefaultAsync();

        if (manager == null)
        {
            throw new NotFoundElementException($"Менеджер с id = {userId} не найден");
        }

        return manager.RestaurantId;
    }
    
    public async Task<Guid?> GetRestaurantIdForCook(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();
        
        if (user == null)
        {
            throw new NotFoundElementException($"Пользователь с id = {userId} не найден");
        }

        var roleIdCook = await _context.Roles
            .Where(r => r.Name == UserRoles.Cook)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        var roleCook = await _context.UserRoles
            .Where(o => o.UserId == userId && o.RoleId == roleIdCook)
            .FirstOrDefaultAsync();

        if (roleCook == null)
        {
            throw new NotFoundElementException($"Пользователь с id = {userId} не имеет роль повара");
        }

        var cook = await _context.Cooks
            .Where(c => c.Id == userId)
            .FirstOrDefaultAsync();

        if (cook == null)
        {
            throw new NotFoundElementException($"Менеджер с id = {userId} не найден");
        }

        return cook.RestaurantId;
    }
    
    public async Task DeleteManagerRole(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundElementException($"Не удалось найти пользователя с id = {userId}");
        }

        await _userManager.RemoveFromRoleAsync(user, UserRoles.Manager);
        await _context.SaveChangesAsync();

        await DeleteManager(userId);
    }

    public async Task DeleteCookRole(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundElementException($"Не удалось найти пользователя с id = {userId}");
        }

        await _userManager.RemoveFromRoleAsync(user, UserRoles.Cook);
        await _context.SaveChangesAsync();

        await DeleteCook(userId);
    }

    public async Task DeleteCourierRole(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundElementException($"Не удалось найти пользователя с id = {userId}");
        }

        await _userManager.RemoveFromRoleAsync(user, UserRoles.Courier);
        await _context.SaveChangesAsync();

        await DeleteCourier(userId);
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