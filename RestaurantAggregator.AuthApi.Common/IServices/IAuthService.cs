using System.Security.Claims;
using RestaurantAggregator.AuthApi.Common.DTO;

namespace RestaurantAggregator.AuthApi.Common.IServices;

public interface IAuthService
{
    Task RegisterAdmin();
    Task<ClaimsIdentity> LoginAdmin(LoginCredentialDto model);
    Task<TokenPairDto> Login(LoginCredentialDto model);
    Task<TokenPairDto> RefreshToken(TokenPairDto oldTokens);
    Task<TokenPairDto> RegisterCustomer(RegisterCustomerCredentialDto model);
    Task<TokenPairDto> RegisterWorkerAsCustomer(LoginCredentialDto model, string address);
    Task Logout(string userId);
}