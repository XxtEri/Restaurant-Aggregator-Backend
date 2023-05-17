using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.AdminPanel.Models;

public class UpdateInfoUserProfileModel
{
    [Required]
    [MinLength(1)]
    public string Username { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; }
    
    public DateTime? BirthDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [MaybeNull]
    public string Phone { get; set; }

    [MaybeNull] 
    public string Address { get; set; }
}