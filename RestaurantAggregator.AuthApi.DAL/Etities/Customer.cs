using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.AuthApi.DAL.Etities;

public class Customer : User
{
    [Required] 
    public string Address { get; set; }
}