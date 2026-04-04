using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Web.Controllers;
using Web.Data;
using Web.Models;

namespace Web.Tests;

[TestFixture]
public class ExercisesControllerTests
{
    private AppDbContext _context;
    private ExercisesController _controller;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _controller = new ExercisesController(_context);
    }

    [Test]
    public async Task Index_ReturnsViewWithModel()
    {
        // Arrange
        var cat = new Category { Id = 1, Name = "Силови" };
        _context.Categories.Add(cat);
        _context.Exercises.Add(new Exercise { Name = "Test Ex", CategoryId = 1 });
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        var model = viewResult?.Model as List<Exercise>;
        Assert.That(model?.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task Edit_ReturnsNotFound_WhenIdIsNull()
    {
        // Act
        var result = await _controller.Edit(null);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task Create_Post_ValidModel_SavesAndRedirects()
    {
        // Arrange
        var exercise = new Exercise { Name = "New Exercise", CategoryId = 1 };

        // Act
        var result = await _controller.Create(exercise);

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        Assert.That(_context.Exercises.Count(), Is.EqualTo(1));
    }

    [TearDown]
    public void TearDown() 
    {
        _context.Dispose();
        _controller.Dispose();
    }
}