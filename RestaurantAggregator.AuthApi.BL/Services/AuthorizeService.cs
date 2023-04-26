using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using RestaurantAggregator.AuthApi.BL.Managers;
using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.AuthApi.DAL.Etities;
using RestaurantAggregator.CommonFiles;

namespace RestaurantAggregator.AuthApi.BL.Services;

public class AuthorizeService: IAuthorizeServise
{
    private readonly UserManager<User> _userManager;

    public AuthorizeService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<TokenPairDto> Login(LoginCredentialDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        
        if (user == null)
        {
            //неверный логин или пароль
        }

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
        {
            //неверный логин или пароль
        }

        if (user.Banned)
        {
            //ваш аккаунт заблокирован
        }

        return await GetTokenPair(user);
    }

    public async Task<TokenPairDto> Refresh(TokenPairDto oldTokens)
    {
        var userId = TokenManager.GetIdOldToken(oldTokens.AccessToken);
        
        if (userId == null)
        {
            //неверный токен
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null ||
            user.RefreshToken != oldTokens.RefreshToken ||
            user.RefreshTokenExpires <= DateTime.Now)
        {
            //неверный токен
        }

        if (user.Banned)
        {
            //аккаунт заблокирован
        }

        return await GetTokenPair(user);
    }

    public async Task Logout(string userId)
    {
        var user = await _userManager.FindByEmailAsync(userId);

        if (user == null)
        {
            //неверный токен
        }

        user.RefreshToken = null;
        user.RefreshTokenExpires = null;

        await _userManager.UpdateAsync(user);
    }

    private async Task<TokenPairDto> GetTokenPair(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var accessToken = TokenManager.CreateAccessToken(claims);
        var refreshToken = TokenManager.CreateRefreshToken(claims);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = DateTime.Now.AddDays(JwtConfigs.RefreshTime);

        await _userManager.UpdateAsync(user);

        return new TokenPairDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}