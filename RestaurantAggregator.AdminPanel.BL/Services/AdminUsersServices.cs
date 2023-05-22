using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.AdminPanel.BL.Services;

public class AdminUsersServices: IAdminUsersServices
{
    private readonly IAdminProfileService _profileService;
    private readonly IAdminRestaurantsService _adminRestaurantsService;
    private readonly IUserService _userService;

    public AdminUsersServices(IAdminProfileService profileService, 
        IAdminRestaurantsService adminRestaurantsService,
        IUserService userService)
    {
        _profileService = profileService;
        _adminRestaurantsService = adminRestaurantsService;
        _userService = userService;
    }
    
    public async Task<List<UserDto>> Select()
    {
        return await _profileService.GetUsers();
    }

    public async Task<UserDto> Get(Guid id)
    {
        return await _profileService.GetUser(id);
    }

    public async Task ChangeStatusBannedUser(Guid userId)
    {
        await _profileService.ChangeStatusBannedUser(userId);
    }

    public async Task ChangeInfoUserProfile(Guid userId, UpdateInfoUserProfileDto model)
    {
        await _profileService.ChangeUser(userId, model);
    }

    public async Task Create(RegisterUserCredentialDto model)
    {
        await _profileService.RegisterUser(model);
    }

    public async Task Delete(Guid id)
    {
        await _profileService.DeleteUser(id);
    }

    public async Task AddManagerRole(Guid id)
    {
        await _profileService.RegisterManager(id);
        await _userService.AddNewManagerToDb(id);
    }

    public async Task AddCookRole(Guid id)
    {
        await _profileService.RegisterCook(id);
        await _userService.AddNewCookToDb(id);
    }

    public async Task AddCourierRole(Guid id)
    {
        await _profileService.RegisterCourier(id);
        await _userService.AddNewCourierToDb(id);
    }

    public async Task<Guid?> GetRestaurantIdForManager(Guid userId)
    {
        return await _profileService.GetRestaurantIdForManager(userId);
    }
    
    public async Task<Guid?> GetRestaurantIdForCook(Guid userId)
    {
        return await _profileService.GetRestaurantIdForCook(userId);
    }

    public async Task AppointManagerInRestaurant(Guid managerId, Guid restaurantId)
    {
        var isValidRestaurantId = await _adminRestaurantsService.CheckIsIdRestaurant(restaurantId);

        if (isValidRestaurantId)
        {
            await _profileService.AppointManagerInRestaurant(managerId, restaurantId);
            await _userService.AddRestaurantIdForManager(managerId, restaurantId);

            var restaurantIdOfManager = await GetRestaurantIdForManager(managerId);
            if (restaurantIdOfManager != null)
            {
                await _adminRestaurantsService.DeleteManagerInRestaurant(new Guid(restaurantIdOfManager.ToString()!));
            }
            
            await _adminRestaurantsService.AddManagerToRestaurant(managerId, restaurantId);
        }
    }
    
    public async Task AppointCookInRestaurant(Guid cookId, Guid restaurantId)
    {
        var isValidRestaurantId = await _adminRestaurantsService.CheckIsIdRestaurant(restaurantId);

        if (isValidRestaurantId)
        {
            await _profileService.AppointCookInRestaurant(cookId, restaurantId);
            await _userService.AddRestaurantIdForCook(cookId, restaurantId);

            var restaurantIdOfCook = await GetRestaurantIdForCook(cookId);
            if (restaurantIdOfCook != null)
            {
                await _adminRestaurantsService.DeleteCookInRestaurant(new Guid(restaurantIdOfCook.ToString()!));
            }
            
            await _adminRestaurantsService.AddCookToRestaurant(cookId, restaurantId);
        }
    }

    public async Task DeleteManagerRole(Guid userId)
    {
        await _profileService.DeleteManagerRole(userId);
        await _userService.DeleteManagerFromDb(userId);
    }

    public async Task DeleteCookRole(Guid userId)
    {
        await _profileService.DeleteCookRole(userId);
        await _userService.DeleteCookFromDb(userId);
    }

    public async Task DeleteCourierRole(Guid userId)
    {
        await _profileService.DeleteCourierRole(userId);
        await _userService.DeleteCourierFromDb(userId);
    }
}