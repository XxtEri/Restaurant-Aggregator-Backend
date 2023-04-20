using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.AuthApi.Common.DTO;

public class LoginCredentialDTO
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}