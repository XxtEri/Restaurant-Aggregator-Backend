using RestaurantAggregator.AuthApi.Common.DTO;

namespace RestaurantAggregator.AuthApi.Common.IServices;

public interface IProfileService
{
    Task<CustomerProfileDto> GetCustomerProfile(string userId);
    Task ChangeInfoCustomerProfile(string userId, ChangeInfoCustomerProfileDto model);
    Task ChangePassword(string userId, ChangePasswordDto model);
}