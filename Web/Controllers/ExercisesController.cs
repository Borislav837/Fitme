using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace Web.Controllers;

public class ExercisesController : Controller
{
    private readonly AppDbContext _context;

    public ExercisesController(AppDbContext context) => _context = context;

    // READ: List all exercises with their Category
    public async Task<IActionResult> Index()
    {
        var exercises = await _context.Exercises
            .Include(e => e.Category) // Join to show "Силови", "Кардио", etc.
            .OrderBy(e => e.Name)
            .ToListAsync();
        return View(exercises);
    }

    // CREATE: Show the form
    public IActionResult Create()
    {
        // Populate a dropdown with Categories
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
        return View();
    }

    // CREATE: Save to DB
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Exercise exercise)
    {
        if (ModelState.IsValid)
        {
            _context.Add(exercise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(exercise);
    }
    
    
    // GET: Exercises/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var exercise = await _context.Exercises.FindAsync(id);
        if (exercise == null) return NotFound();

        // Populate dropdown with Bulgarian Categories for the edit form
        ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", exercise.CategoryId);
        return View(exercise);
    }

// POST: Exercises/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Exercise exercise)
    {
        if (id != exercise.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(exercise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    
        ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", exercise.CategoryId);
        return View(exercise);
    }

// GET: Exercises/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var exercise = await _context.Exercises
            .Include(e => e.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (exercise == null) return NotFound();

        return View(exercise);
    }

// POST: Exercises/Delete/5
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var exercise = await _context.Exercises.FindAsync(id);
        if (exercise != null)
        {
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
