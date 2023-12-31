using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.AuthApi.Common.DTO;

public class RegisterCustomerCredentialDto
{
    [Required]
    public string Username { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [Phone]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
    
    [Required] 
    public string Address { get; set; }
}