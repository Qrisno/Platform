﻿using Microsoft.EntityFrameworkCore;
using Platform.Domain.Entities.Models;
using Platform.Infrastructure.Interfaces;

namespace Platform.Infrastructure
{
    public class PlatformDbContext(DbContextOptions<PlatformDbContext> options) : DbContext(options), IPlatformDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Auth> AuthUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
    }
}