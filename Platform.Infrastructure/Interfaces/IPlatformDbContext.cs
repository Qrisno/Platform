using Microsoft.EntityFrameworkCore;
using Platform.Domain.Entities.Models;

namespace Platform.Infrastructure.Interfaces
{
    public interface IPlatformDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Auth> AuthUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}