using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace RestaurantAggregator.AuthApi.DAL.Etities;

public class User: IdentityUser<long>
{
    [Key]
    public Guid Id { get; set; }
    [MinLength(1)]
    [Required]
    public string FullName { get; set; }
    [DataType(DataType.Date)]
    [MaybeNull]
    public string BirthDate { get; set; }
    [Phone]
    [MaybeNull]
    public string Gender { get; set; }
    [MaybeNull]
    public string Phone { get; set; }
    [EmailAddress]
    [MaybeNull]
    public string Email { get; set; }
    [MinLength(1)]
    public string Password { get; set; }
}