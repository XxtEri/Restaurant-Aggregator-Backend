using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.API.DAL.Entities;

public class Customer
{
    [Key]
    public Guid Id { get; set; }
}