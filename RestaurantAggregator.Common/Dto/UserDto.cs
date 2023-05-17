using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.CommonFiles.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Username { get; set; }
    
    public string? Role { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; }
    
    public DateTime? BirthDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [MaybeNull]
    public string Phone { get; set; }

    [Required]
    public bool isCourier { get; set; }
    
    [MaybeNull] 
    public string Address { get; set; }
    
    public Guid? ManagerRestaurantId { get; set; }
    
    public Guid? CookRestaurantId { get; set; }
}