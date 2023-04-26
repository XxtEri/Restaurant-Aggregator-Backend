using RestaurantAggregator.AuthApi.Common.DTO;

namespace RestaurantAggregator.AuthApi.Common.IServices;

public interface IRegisterService
{
    Task<TokenPairDto> RegisterCustomer(RegisterCustomerCredentialDto model);
    Task<TokenPairDto> RegisterWorkerAsCustomer(LoginCredentialDto model, string address);
}