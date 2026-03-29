using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;
using RentACar.ViewModels;
using System.Runtime.ConstrainedExecution;

namespace RentACar.Controllers;

public class CarsController : Controller
{
    private readonly AppDbContext _db;

    public CarsController(AppDbContext db)
    {
        _db = db;
    }

    private bool IsAuthenticated => HttpContext.Session.GetInt32("UserId") != null;
    private bool IsAdmin => HttpContext.Session.GetString("Role") == "Admin";

    // GET: /Cars - public listing
    public async Task<IActionResult> Index()
    {
        var cars = await _db.Cars.ToListAsync();
        return View(cars);
    }

    // GET: /Cars/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var car = await _db.Cars.FindAsync(id);
        if (car == null) return NotFound();
        return View(car);
    }

    // Admin: Create
    public IActionResult Create()
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Car car)
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        if (!ModelState.IsValid) return View(car);

        _db.Cars.Add(car);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index", "Admin");
    }

    // Admin: Edit
    public async Task<IActionResult> Edit(int id)
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        var car = await _db.Cars.FindAsync(id);
        if (car == null) return NotFound();
        return View(car);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Car car)
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        if (!ModelState.IsValid) return View(car);

        _db.Cars.Update(car);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index", "Admin");
    }

    // Admin: Delete
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        var car = await _db.Cars.FindAsync(id);
        if (car == null) return NotFound();
        return View(car);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!IsAdmin) return RedirectToAction("Login", "Account");
        var car = await _db.Cars.FindAsync(id);
        if (car != null)
        {
            _db.Cars.Remove(car);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index", "Admin");
    }
}

