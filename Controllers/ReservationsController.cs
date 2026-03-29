using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;
using RentACar.ViewModels;

namespace RentACar.Controllers;

public class ReservationsController : Controller
{
    private readonly AppDbContext _db;

    public ReservationsController(AppDbContext db)
    {
        _db = db;
    }

    private int? UserId => HttpContext.Session.GetInt32("UserId");
    private bool IsAdmin => HttpContext.Session.GetString("Role") == "Admin";

    // Search for available cars
    public IActionResult Search()
    {
        if (UserId == null) return RedirectToAction("Login", "Account");
        return View(new SearchCarsViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Search(SearchCarsViewModel model)
    {
        if (UserId == null) return RedirectToAction("Login", "Account");

        if (model.StartDate < DateTime.Today)
            ModelState.AddModelError("StartDate", "Началната дата не може да е в миналото.");

        if (model.EndDate <= model.StartDate)
            ModelState.AddModelError("EndDate", "Крайната дата трябва да е след началната.");

        if (!ModelState.IsValid) return View(model);

        // Find cars not reserved during the requested period
        var reservedCarIds = await _db.Reservations
            .Where(r => r.Status != "Rejected" &&
                        r.StartDate < model.EndDate &&
                        r.EndDate > model.StartDate)
            .Select(r => r.CarId)
            .Distinct()
            .ToListAsync();

        model.AvailableCars = await _db.Cars
            .Where(c => !reservedCarIds.Contains(c.Id))
            .ToListAsync();

        model.Searched = true;
        return View(model);
    }

    // GET: Confirm reservation
    public async Task<IActionResult> Create(int carId, DateTime startDate, DateTime endDate)
    {
        if (UserId == null) return RedirectToAction("Login", "Account");
        var car = await _db.Cars.FindAsync(carId);
        if (car == null) return NotFound();

        return View(new CreateReservationViewModel
        {
            CarId = carId,
            StartDate = startDate,
            EndDate = endDate,
            Car = car
        });
    }

    // POST: Create reservation
    [HttpPost]
    public async Task<IActionResult> Create(CreateReservationViewModel model)
    {
        if (UserId == null) return RedirectToAction("Login", "Account");

        // Double-check availability (race condition protection)
        bool isAvailable = !await _db.Reservations.AnyAsync(r =>
            r.CarId == model.CarId &&
            r.Status != "Rejected" &&
            r.StartDate < model.EndDate &&
            r.EndDate > model.StartDate);

        if (!isAvailable)
        {
            TempData["Error"] = "За съжаление, автомобилът вече е зает за избрания период.";
            return RedirectToAction("Search");
        }

        var reservation = new Reservation
        {
            UserId = UserId.Value,
            CarId = model.CarId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Status = "Pending"
        };

        _db.Reservations.Add(reservation);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Заявката е подадена успешно! Очаквайте одобрение.";
        return RedirectToAction("MyReservations");
    }

    // My reservations
    public async Task<IActionResult> MyReservations()
    {
        if (UserId == null) return RedirectToAction("Login", "Account");

        var reservations = await _db.Reservations
            .Include(r => r.Car)
            .Where(r => r.UserId == UserId)
            .OrderByDescending(r => r.StartDate)
            .ToListAsync();

        return View(reservations);
    }
}
