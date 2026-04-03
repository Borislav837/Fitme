using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var bulgarianCulture = new System.Globalization.CultureInfo("bg-BG");

        var allLogs = await _db.WorkoutLogs
            .Include(l => l.Workout)
            .Include(l => l.Exercise)
            .ThenInclude(e => e.Category)
            .ToListAsync();

        var monthlyStats = allLogs
            .GroupBy(l => l.Workout.Date.ToString("MMMM yyyy", bulgarianCulture))
            .Select(g => new
            {
                Month = g.Key,
                // Count exercises belonging to Category "Силови"
                StrengthCount = g.Count(x => x.Exercise.Category.Name == "Силови"),
                // Count exercises belonging to Category "Кардио"
                CardioCount = g.Count(x => x.Exercise.Category.Name == "Кардио"),
                // Count exercises belonging to Category "Гъвкавост"
                StretchCount = g.Count(x => x.Exercise.Category.Name == "Гъвкавост"),
                // Sum(Sets * Reps * Weight) - handling nulls as 0
                TotalWeight = g.Sum(x => (x.Sets ?? 0) * (x.Reps ?? 0) * (x.Weight ?? 0))
            })
            .ToList();

        return View(monthlyStats);
    }

}