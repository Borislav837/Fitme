using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace Web.Controllers;

public class WorkoutsController : Controller
{
    private readonly AppDbContext _context;

    public WorkoutsController(AppDbContext context) => _context = context;

    // GET: Workouts
    public async Task<IActionResult> Index()
    {
        // .Include and .ThenInclude are vital for relational data!
        var workouts = await _context.Workouts
            .Include(w => w.WorkoutLogs)
            .ThenInclude(log => log.Exercise)
            .OrderByDescending(w => w.Date)
            .ToListAsync();

        return View(workouts);
    }
    
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var workout = await _context.Workouts
            .Include(w => w.WorkoutLogs)
            .ThenInclude(l => l.Exercise) // Join to get Bulgarian Names
            .FirstOrDefaultAsync(m => m.Id == id);

        //if (workout == null) return NotFound();

        return View(workout);
    }

// GET: Workouts/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Workout workout)
    {
        if (ModelState.IsValid)
        {
            _context.Add(workout);
            await _context.SaveChangesAsync();
            // Redirect to the Edit page to add logs
            return RedirectToAction(nameof(Edit), new { id = workout.Id });
        }
        return View(workout);
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var workout = await _context.Workouts
            .Include(w => w.WorkoutLogs).ThenInclude(l => l.Exercise)
            .FirstOrDefaultAsync(w => w.Id == id);

        ViewBag.ExerciseId = new SelectList(_context.Exercises, "Id", "Name");
        return View(workout);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditWorkoutHeader(Workout workout)
    {
        if (ModelState.IsValid)
        {
            // 1. Find the existing workout in the database
            var existingWorkout = await _context.Workouts.FindAsync(workout.Id);
        
            if (existingWorkout == null) return NotFound();

            // 2. Update only the header fields
            existingWorkout.Name = workout.Name;
            existingWorkout.Date = workout.Date;

            // 3. Save changes
            _context.Update(existingWorkout);
            await _context.SaveChangesAsync();

            // 4. Stay on the Edit page to allow adding logs
            return RedirectToAction(nameof(Edit), new { id = workout.Id });
        }

        // If something fails, reload the edit view
        return RedirectToAction(nameof(Edit), new { id = workout.Id });
    }
    
// GET: Workouts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var workout = await _context.Workouts
            .FirstOrDefaultAsync(m => m.Id == id);

        if (workout == null) return NotFound();

        return View(workout);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var workout = await _context.Workouts
            .Include(w => w.WorkoutLogs) // Crucial: Include logs to remove them too
            .FirstOrDefaultAsync(m => m.Id == id);

        if (workout != null)
        {
            // Remove the logs first, then the workout
            _context.WorkoutLogs.RemoveRange(workout.WorkoutLogs);
            _context.Workouts.Remove(workout);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AddLog(WorkoutLog log)
    {
        _context.WorkoutLogs.Add(log);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Edit), new { id = log.WorkoutId });
    }

    public async Task<IActionResult> DeleteLog(int id)
    {
        var log = await _context.WorkoutLogs.FindAsync(id);
        int? workoutId = log.WorkoutId;
        _context.WorkoutLogs.Remove(log);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Edit), new { id = workoutId });
    }

}