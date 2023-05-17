using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.CommonFiles.Dto;

public class RegisterUserCredentialDto
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    
    public DateTime? BirthDate { get; set; }
    
    [Required(ErrorMessage = "Gender is required")]
    public Gender Gender { get; set; }
    
    [MaybeNull]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}