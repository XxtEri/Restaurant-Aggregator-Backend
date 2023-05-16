using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.AuthApi.Common.DTO;

public class ChangePasswordDto
{
    [Required]
    public string OldPassword { get; set; }
    [Required]
    public string NewPassword { get; set; }
}