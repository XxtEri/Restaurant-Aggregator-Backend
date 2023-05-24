using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.APIAuth.Models;

public class RegisterCustomerCredentialModel
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
    [MinLength(6)]
    public string? Password { get; set; }
    
    [Required] 
    [MinLength(2)]
    public string Address { get; set; }
}