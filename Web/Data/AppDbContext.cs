using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    
    public DbSet<Workout> Workouts { get; set; }
    
    public DbSet<WorkoutLog> WorkoutLogs { get; set; }
}