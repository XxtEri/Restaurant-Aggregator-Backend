using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAggregator.API.DAL.Entities;

public class Manager
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("Restaurant")]
    public Guid RestaurantId { get; set; }
    
    public Restaurant Restaurant { get; set; }
}