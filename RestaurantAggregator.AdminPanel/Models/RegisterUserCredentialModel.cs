using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.AdminPanel.Models;

public class RegisterUserCredentialModel
{
    [Required(ErrorMessage = "Введите ваше имя")]
    [RegularExpression(@"[A-Za-zА-Яа-я0-9]+$", ErrorMessage = "Имя должно состоять только из цифр или букв латинского или русского алфавита")]
    public string Username { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Введите email")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Введите дату рождения")]
    public DateTime BirthDate { get; set; }
    
    [Required(ErrorMessage = "Выберите ваш пол")]
    public Gender Gender { get; set; }
    
    [Required(ErrorMessage = "Введите номер телефона")]
    [RegularExpression(@"^\+?\d{0,2}\-?\d{3}\-?\d{3}\-?\d{2}\-?\d{2}$", ErrorMessage = "Введите правильный номер телефона")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    public string Password { get; set; }
}