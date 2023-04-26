using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.AuthApi.Common.DTO;

public class UserProfileDto
{
    public Guid Id { get; set; }
    
    [Required]
    [EmailAddress]
    public string FullName { get; set; }
    
    [Required]
    public DateTime BirtDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [Phone]
    [MaybeNull]
    public string Phone { get; set; }
    
    [MaybeNull]
    public string Address { get; set; }
    
    public Guid? ManagerRestaurantId { get; set; }
    
    public Guid? CookRestaurantId { get; set; }

    [Required] 
    public bool IsCourier { get; set; } = false;
}