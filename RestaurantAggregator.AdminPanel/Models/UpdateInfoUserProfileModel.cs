using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.AdminPanel.Models;

public class UpdateInfoUserProfileModel
{
    [Required(ErrorMessage = "Введите ваше имя")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Введите дату рождения")]
    public DateTime BirthDate { get; set; }
    
    [Required(ErrorMessage = "Выберите ваш пол")]
    public Gender Gender { get; set; }
    
    [Required(ErrorMessage = "Введите номер телефона")]
    [RegularExpression(@"^\+?\d{0,2}\-?\d{3}\-?\d{3}\-?\d{2}\-?\d{2}$", ErrorMessage = "Введите правильный номер телефона")]
    public string Phone { get; set; }
    
    public string? Address { get; set; }
}