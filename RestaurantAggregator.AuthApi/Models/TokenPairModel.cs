using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.APIAuth.Models;

public class TokenPairModel
{
    [Required]
    public string AccessToken { get; set; }
    
    [Required]
    public string? RefreshToken { get; set; }
}