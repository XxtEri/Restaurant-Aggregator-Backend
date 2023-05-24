using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.AuthApi.DAL.Etities;

public class User: IdentityUser<Guid>
{
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpires { get; set; }
    
    public bool Banned { get; set; } = false;
    
    public Cook Cook { get; set; }
    
    public Customer Customer { get; set; }

    public Manager Manager { get; set; }
    
    public Courier Courier { get; set; }
}