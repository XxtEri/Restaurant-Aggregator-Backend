using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.API.DAL.Entities;

public class Manager
{
    [Key]
    public Guid Id { get; set; }
}