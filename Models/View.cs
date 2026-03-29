using System.ComponentModel.DataAnnotations;
using RentACar.Models;

namespace RentACar.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Въведете потребителско име.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Въведете парола.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "Потребителското име е задължително.")]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Паролата е задължителна.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Паролата трябва да е поне 6 символа.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Името е задължително.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Фамилията е задължителна.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "ЕГН-то е задължително.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "ЕГН-то трябва да съдържа точно 10 цифри.")]
    public string Egn { get; set; } = string.Empty;

    [Required(ErrorMessage = "Телефонният номер е задължителен.")]
    [RegularExpression(@"^(\+359|0)\d{8,9}$", ErrorMessage = "Невалиден телефонен номер.")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Имейлът е задължителен.")]
    [EmailAddress(ErrorMessage = "Невалиден имейл адрес.")]
    public string Email { get; set; } = string.Empty;
}

public class SearchCarsViewModel
{
    [Required(ErrorMessage = "Изберете начална дата.")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Изберете крайна дата.")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);

    public List<Car> AvailableCars { get; set; } = new();
    public bool Searched { get; set; } = false;
}

public class CreateReservationViewModel
{
    public int CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Car? Car { get; set; }
}

public class AdminUserViewModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Egn { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }
}
