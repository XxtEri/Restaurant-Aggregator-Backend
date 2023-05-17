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
    
    public async Task<List<UserDto>> Get()
    {
        return await _profileService.GetUser();
    }
    
    public Task Create(RegisterUserCredentialDto model)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}