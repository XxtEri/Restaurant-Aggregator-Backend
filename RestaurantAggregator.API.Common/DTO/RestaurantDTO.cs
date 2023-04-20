using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.Common.DTO;

public class RestaurantDTO
{
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [Required]
    public string Name { get; set; }
    
    [MaybeNull]
    public List<MenuDTO> Menu { get; set; }
    
    [MaybeNull]
    public ICollection<CookDTO> Cooks { get; set; }
    
    [MaybeNull]
    public ICollection<ManagerDTO> Managers { get; set; }
}