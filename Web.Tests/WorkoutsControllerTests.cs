using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Web.Controllers;
using Web.Data;
using Web.Models;

namespace Web.Tests;

[TestFixture]
public class WorkoutsControllerTests
{
    private AppDbContext _context;
    private WorkoutsController _controller;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _controller = new WorkoutsController(_context);
    }

    [Test]
    public async Task DeleteConfirmed_RemovesWorkoutAndLogs()
    {
        // Arrange: Create Workout with a Log
        var workout = new Workout { Id = 1, Name = "Test Workout", Date = DateTime.Now };
        var log = new WorkoutLog { Id = 1, WorkoutId = 1, ExerciseId = 1 };
        
        _context.Workouts.Add(workout);
        _context.WorkoutLogs.Add(log);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.DeleteConfirmed(1);

        // Assert
        Assert.That(_context.Workouts.Find(1), Is.Null);
        Assert.That(_context.WorkoutLogs.Find(1), Is.Null);
        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
    }

    [Test]
    public async Task Details_ReturnsNotFound_WhenIdIsNull()
    {
        // Act
        var result = await _controller.Details(null);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task AddLog_SavesAndRedirects()
    {
        // Arrange
        var log = new WorkoutLog { WorkoutId = 1, ExerciseId = 1, Sets = 3 };

        // Act
        var result = await _controller.AddLog(log);

        // Assert
        Assert.That(_context.WorkoutLogs.Count(), Is.EqualTo(1));
        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
    }

    [TearDown]
    public void TearDown() 
    {
        _context.Dispose();
        _controller.Dispose();
    }
}