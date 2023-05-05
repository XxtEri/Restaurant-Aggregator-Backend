using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.API.DAL.Entities;

public class Cook
{
    [Key]
    public Guid Id { get; set; }
}