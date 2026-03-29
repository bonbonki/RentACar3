using Microsoft.EntityFrameworkCore;
using RentACar.Models;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;

namespace RentACar.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Unique constraints
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Egn).IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<Car>()
            .Property(c => c.PricePerDay)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reservations)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Car)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.CarId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed admin user
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "admin",
            PasswordHash = "$2a$11$R9h/lIPzHZ7vGTEyNm9I6eVUAIh5S8.9mB.Dsn.L8Y8E1m6tXN.7G",
            FirstName = "Администратор",
            LastName = "Системен",
            Egn = "0000000000",
            Phone = "+35900000000",
            Email = "admin@rentacar.bg",
            Role = "Admin"
        });

        // Seed some cars
        modelBuilder.Entity<Car>().HasData(
            new Car { Id = 1, Brand = "Toyota", Model = "Corolla", Year = 2022, Seats = 5, PricePerDay = 60, Description = "Икономичен седан, автоматична скоростна кутия, климатик." },
            new Car { Id = 2, Brand = "BMW", Model = "3 Series", Year = 2023, Seats = 5, PricePerDay = 120, Description = "Луксозен седан, кожен салон, навигация." },
            new Car { Id = 3, Brand = "Volkswagen", Model = "Passat", Year = 2021, Seats = 5, PricePerDay = 80, Description = "Комфортен семеен автомобил." },
            new Car { Id = 4, Brand = "Ford", Model = "Focus", Year = 2020, Seats = 5, PricePerDay = 55, Description = "Компактен хечбек, икономичен двигател." },
            new Car { Id = 5, Brand = "Mercedes", Model = "GLC", Year = 2023, Seats = 5, PricePerDay = 180, Description = "Луксозен SUV, 4MATIC задвижване." }
        );
    }
}
