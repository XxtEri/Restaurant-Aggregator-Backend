using System.Security.Claims;
using RestaurantAggregator.AdminPanel.Common.Dto;

namespace RestaurantAggregator.AdminPanel.Common.Interfaces;

public interface IAuthService
{
    Task<ClaimsIdentity> Login(LoginCredentialDto model);
}