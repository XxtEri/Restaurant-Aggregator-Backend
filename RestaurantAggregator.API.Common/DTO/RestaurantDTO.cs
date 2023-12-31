using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.Common.DTO;

public class RestaurantDTO
{
    public Guid Id { get; set; }
    
    [Display(Name = "Название")]
    [MinLength(1)]
    [Required]
    public string Name { get; set; }
    
    public List<MenuDTO> Menus { get; set; }

    public RestaurantDTO()
    {
        Menus = new List<MenuDTO>();
    }
}