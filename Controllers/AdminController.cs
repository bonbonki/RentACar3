using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;
using RentACar.ViewModels;

namespace RentACar.Controllers;

public class AdminController : Controller
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db)
    {
        _db = db;
    }

    private bool IsAdmin => HttpContext.Session.GetString("Role") == "Admin";

    private IActionResult RequireAdmin()
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        return null!;
    }

    public async Task<IActionResult> Index()
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");

        ViewBag.UserCount = await _db.Users.CountAsync();
        ViewBag.CarCount = await _db.Cars.CountAsync();
        ViewBag.PendingCount = await _db.Reservations.CountAsync(r => r.Status == "Pending");
        ViewBag.ApprovedCount = await _db.Reservations.CountAsync(r => r.Status == "Approved");

        return View();
    }

    // ---- USERS ----

    public async Task<IActionResult> Users()
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        var users = await _db.Users.ToListAsync();
        return View(users);
    }

    public async Task<IActionResult> EditUser(int id)
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();

        return View(new AdminUserViewModel
        {
            Id = user.Id,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Egn = user.Egn,
            Phone = user.Phone,
            Email = user.Email,
            Role = user.Role
        });
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(AdminUserViewModel model)
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");

        var user = await _db.Users.FindAsync(model.Id);
        if (user == null) return NotFound();

        // Unique checks (excluding current user)
        if (await _db.Users.AnyAsync(u => u.Username == model.Username && u.Id != model.Id))
            ModelState.AddModelError("Username", "Потребителското име вече е заето.");
        if (await _db.Users.AnyAsync(u => u.Egn == model.Egn && u.Id != model.Id))
            ModelState.AddModelError("Egn", "Вече съществува потребител с това ЕГН.");
        if (await _db.Users.AnyAsync(u => u.Email == model.Email && u.Id != model.Id))
            ModelState.AddModelError("Email", "Имейлът вече е регистриран.");

        if (!ModelState.IsValid) return View(model);

        user.Username = model.Username;
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Egn = model.Egn;
        user.Phone = model.Phone;
        user.Email = model.Email;
        user.Role = model.Role;

        if (!string.IsNullOrWhiteSpace(model.NewPassword))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

        await _db.SaveChangesAsync();
        return RedirectToAction("Users");
    }

    public async Task<IActionResult> DeleteUser(int id)
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost, ActionName("DeleteUser")]
    public async Task<IActionResult> DeleteUserConfirmed(int id)
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        var user = await _db.Users.FindAsync(id);
        if (user != null)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Users");
    }

    // ---- RESERVATIONS ----

    public async Task<IActionResult> Reservations()
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");

        var reservations = await _db.Reservations
            .Include(r => r.User)
            .Include(r => r.Car)
            .OrderByDescending(r => r.StartDate)
            .ToListAsync();

        return View(reservations);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveReservation(int id)
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        var r = await _db.Reservations.FindAsync(id);
        if (r != null) { r.Status = "Approved"; await _db.SaveChangesAsync(); }
        return RedirectToAction("Reservations");
    }

    [HttpPost]
    public async Task<IActionResult> RejectReservation(int id)
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        var r = await _db.Reservations.FindAsync(id);
        if (r != null) { r.Status = "Rejected"; await _db.SaveChangesAsync(); }
        return RedirectToAction("Reservations");
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        var r = await _db.Reservations.FindAsync(id);
        if (r != null) { _db.Reservations.Remove(r); await _db.SaveChangesAsync(); }
        return RedirectToAction("Reservations");
    }
}
