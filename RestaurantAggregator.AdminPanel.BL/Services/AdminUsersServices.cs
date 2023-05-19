using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.AdminPanel.BL.Services;

public class AdminUsersServices: IAdminUsersServices
{
    private readonly IAdminProfileService _profileService;

    public AdminUsersServices(IAdminProfileService profileService)
    {
        _profileService = profileService;
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
    }

    public async Task AddCookRole(Guid id)
    {
        await _profileService.RegisterCook(id);
    }

    public async Task AddCourierRole(Guid id)
    {
        await _profileService.RegisterCourier(id);
    }

    public async Task<Guid?> GetRestaurantIdForManager(Guid userId)
    {
        return await _profileService.GetRestaurantIdForManager(userId);
    }
    
    public async Task<Guid> GetRestaurantIdForCook(Guid userId)
    {
        return await _profileService.GetRestaurantIdForCook(userId);
    }
}