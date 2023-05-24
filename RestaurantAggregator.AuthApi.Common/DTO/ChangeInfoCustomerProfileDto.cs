using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.AuthApi.Common.DTO;

public class ChangeInfoCustomerProfileDto
{
    [Required]
    public string Username { get; set; }

    public DateTime BirthDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [MaybeNull]
    [Phone]
    public string? Phone { get; set; }

    [Required] 
    public string Address { get; set; }
}