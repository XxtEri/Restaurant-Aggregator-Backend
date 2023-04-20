using RestaurantAggregator.AuthApi.Common.DTO;

namespace RestaurantAggregator.AuthApi.Common.IServices;

public interface IUserService
{
    public Task<String> LoginUser(LoginCredentialDTO model);
}