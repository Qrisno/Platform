using Microsoft.EntityFrameworkCore;
using Platform.Application.Enums;
using Platform.Application.Models;
using Platform.Application.Repos;
using Platform.Domain.Entities.Models;

namespace Platform.Infrastructure.Repos;

public class CoursesRepository : ICoursesRepository
{
    public PlatformDbContext _dbContext;
    public CoursesRepository(PlatformDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<CourseResponse> GetCoursesByAuthor(int id)
    {
        List<Course> filteredCourses =
            await _dbContext.Courses
            .Where(c => c.AuthorUserId == id)
            .ToListAsync();

        if (filteredCourses.Count == 0)
        {
            return new CourseResponse
            {
                courses = [],
                result = CourseSearchResultEnum.NotFound
            };
        }
        return new CourseResponse
        {
            courses = filteredCourses,
            result = CourseSearchResultEnum.Success
        };

    }

    public async Task<CourseResponse> GetCoursesByStudent(int id)
    {
        var userFound = await _dbContext.Users.FindAsync(id);
        // aq amis checki sachiroa?
        if (userFound == null)
        {
            return new CourseResponse
            {
                courses = [],
                result = CourseSearchResultEnum.UserNotFound
            };
        }

        var courseIds = userFound.Courses;
        List<Course> courses = [];

        foreach (int courseId in courseIds)
        {
            var course = await _dbContext.Courses.FindAsync(courseId);
            if (course != null)
            {
                courses.Add(course);
            }
        }
        return new CourseResponse
        {
            courses = courses,
            result = CourseSearchResultEnum.Success
        };
    }

    public async Task<CourseResponse> AddCourse()
    {

    }
}