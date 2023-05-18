using System.ComponentModel;
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
    
    public List<string> Roles { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; }
    
    public DateTime? BirthDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [MaybeNull]
    [Phone]
    public string Phone { get; set; }
    
    [DefaultValue(false)]
    public bool isBanned { get; set; }
    
    [DefaultValue(false)]
    public bool isCourier { get; set; }
    
    public string? Address { get; set; }
    
    public Guid? ManagerRestaurantId { get; set; }
    
    public Guid? CookRestaurantId { get; set; }
}