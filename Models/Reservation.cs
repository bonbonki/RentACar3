using System.ComponentModel.DataAnnotations;

namespace RentACar.Models;

public class Reservation
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int CarId { get; set; }
    public Car Car { get; set; } = null!;

    [Required(ErrorMessage = "Началната дата е задължителна.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Крайната дата е задължителна.")]
    public DateTime EndDate { get; set; }

    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

    public decimal TotalPrice => (decimal)(EndDate - StartDate).TotalDays * Car.PricePerDay;
}
