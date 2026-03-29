using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;
using RentACar.ViewModels;

namespace RentACar.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _db;

    public AccountController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Невалидно потребителско име или парола.");
            return View(model);
        }

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("Username", user.Username);
        HttpContext.Session.SetString("Role", user.Role);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (await _db.Users.AnyAsync(u => u.Username == model.Username))
            ModelState.AddModelError("Username", "Потребителското име вече е заето.");

        if (await _db.Users.AnyAsync(u => u.Egn == model.Egn))
            ModelState.AddModelError("Egn", "Вече съществува потребител с това ЕГН.");

        if (await _db.Users.AnyAsync(u => u.Email == model.Email))
            ModelState.AddModelError("Email", "Имейлът вече е регистриран.");

        if (!ModelState.IsValid) return View(model);

        var user = new User
        {
            Username = model.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            FirstName = model.FirstName,
            LastName = model.LastName,
            Egn = model.Egn,
            Phone = model.Phone,
            Email = model.Email,
            Role = "User"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("Username", user.Username);
        HttpContext.Session.SetString("Role", user.Role);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
