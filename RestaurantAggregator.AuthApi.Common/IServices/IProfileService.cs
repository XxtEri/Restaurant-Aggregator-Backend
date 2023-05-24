using RestaurantAggregator.AuthApi.Common.DTO;

namespace RestaurantAggregator.AuthApi.Common.IServices;

public interface IProfileService
{
    Task<CustomerProfileDto> GetCustomerProfile(Guid userId);
    Task ChangeInfoCustomerProfile(Guid userId, ChangeInfoCustomerProfileDto model);
    Task ChangePassword(Guid userId, ChangePasswordDto model);
    string? GetUserIdFromToken(string token);
}