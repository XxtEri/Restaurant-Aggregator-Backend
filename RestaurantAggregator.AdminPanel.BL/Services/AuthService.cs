using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using RestaurantAggregator.AdminPanel.Common.Dto;
using RestaurantAggregator.AdminPanel.Common.Interfaces;

namespace RestaurantAggregator.AdminPanel.BL.Services;

public class AuthService: IAuthService
{
    private readonly IAdminUsersServices _adminUsersServices;

    public AuthService(IAdminUsersServices adminUsersServices)
    {
        _adminUsersServices = adminUsersServices;
    }
    
    public async Task<ClaimsIdentity> Login(LoginCredentialDto model)
    {
        return await _adminUsersServices.LoginAdmin(model);
    }
}