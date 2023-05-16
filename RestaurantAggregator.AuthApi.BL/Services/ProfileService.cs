using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.AuthApi.Common.IServices;

namespace RestaurantAggregator.AuthApi.BL.Services;

public class ProfileService: IProfileService
{
    public Task<CustomerProfileDto> GetCustomerProfile(string userId)
    {
        throw new NotImplementedException();
    }

    public Task ChangeInfoCustomerProfile(string userId, ChangeInfoCustomerProfileDto model)
    {
        throw new NotImplementedException();
    }

    public Task ChangePassword(string userId, ChangePasswordDto model)
    {
        throw new NotImplementedException();
    }
}