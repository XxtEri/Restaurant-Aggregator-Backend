using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.AuthApi.Common.DTO;

public class TokenPairDto
{
    [Required]
    public string AccessToken { get; set; }
    
    [Required]
    public string? RefreshToken { get; set; }
}