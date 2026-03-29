using System.ComponentModel.DataAnnotations;

namespace RentACar.Models;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Потребителското име е задължително.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Потребителското име трябва да е между 3 и 50 символа.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Паролата е задължителна.")]
    public string PasswordHash { get; set; } = string.Empty;

    [Required(ErrorMessage = "Името е задължително.")]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Фамилията е задължителна.")]
    [StringLength(50)]
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

    public string Role { get; set; } = "User"; // "User" or "Admin"

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
