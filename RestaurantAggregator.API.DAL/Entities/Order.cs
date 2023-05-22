using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.API.DAL.Entities;

public class Order
{
    [Key]
    public Guid Id { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long NumberOrder { get; set; }
    
    [Required]
    public DateTime DeliveryTime { get; set; }

    [Required]
    public DateTime OrderTime { get; set; }

    [Required] 
    public double Price { get; set; }

    [Required] 
    public OrderStatus Status { get; set; } = OrderStatus.Created;

    [Required]
    [MinLength(2)]
    public string Address { get; set; }

    [ForeignKey("Customer")]
    public Guid CustomerId { get; set; }
    
    [ForeignKey("Cook")]
    public Guid? CookId { get; set; }
    
    [ForeignKey("Courier")]
    public Guid? CourierId { get; set; }
    
    public Cook? Cook { get; set; }
    
    public Courier? Courier { get; set; }
    
    public Customer Customer { get; set; }
    
    public ICollection<OrderDish> OrderDishes { get; set; }

    public Order()
    {
        OrderDishes = new List<OrderDish>();
    }
}