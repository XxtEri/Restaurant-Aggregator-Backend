using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.AuthApi.Common.DTO;

public class TokenDTO
{
    [MaybeNull]
    public string AccessToken { get; set; }
    // [MaybeNull]
    // public string RefreshToken { get; set; }
}