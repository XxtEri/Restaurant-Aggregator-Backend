using System.ComponentModel.DataAnnotations;
using RestaurantAggregator.CommonFiles;

namespace RestaurantAggregator.AdminPanel.Models;

public class ChangeRestaurantIdModel
{
    public Guid UserId { get; set; }
    
    [Required(ErrorMessage = "Введите идентификатор ресторана")]
    public Guid RestaurantId { get; set; }
}