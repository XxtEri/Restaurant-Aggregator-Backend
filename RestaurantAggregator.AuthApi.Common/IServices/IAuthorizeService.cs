using RestaurantAggregator.AuthApi.Common.DTO;

namespace RestaurantAggregator.AuthApi.Common.IServices;

public interface IAuthorizeServise
{
    Task<TokenPairDto> Login(LoginCredentialDto model);
    Task<TokenPairDto> Refresh(TokenPairDto oldTokens);
    Task Logout(string userId);
}