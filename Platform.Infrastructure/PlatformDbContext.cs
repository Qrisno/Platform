using Microsoft.EntityFrameworkCore;
using Platform.Domain.Entities.Models;

namespace Platform.Infrastructure;

public class PlatformDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Auth> AuthUsers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<UserCourse> UserCourses { get; set; }

    public PlatformDbContext(DbContextOptions<PlatformDbContext> options) : base(options)
    {

    }
}
