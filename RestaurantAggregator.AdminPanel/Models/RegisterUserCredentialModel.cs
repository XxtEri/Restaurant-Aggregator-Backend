using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.AdminPanel.Models;

public class RegisterUserCredentialModel
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required(ErrorMessage = "Gender is required")]
    public Gender Gender { get; set; }
    
    [Phone]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}