using System.ComponentModel.DataAnnotations;

namespace RentACar.Models;

public class Car
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Марката е задължителна.")]
    [StringLength(50)]
    public string Brand { get; set; } = string.Empty;

    [Required(ErrorMessage = "Моделът е задължителен.")]
    [StringLength(50)]
    public string Model { get; set; } = string.Empty;

    [Required(ErrorMessage = "Годината е задължителна.")]
    [Range(1900, 2100, ErrorMessage = "Невалидна година.")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Броят места е задължителен.")]
    [Range(1, 20, ErrorMessage = "Броят места трябва да е между 1 и 20.")]
    public int Seats { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Цената е задължителна.")]
    [Range(0.01, 10000, ErrorMessage = "Невалидна цена.")]
    public decimal PricePerDay { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
