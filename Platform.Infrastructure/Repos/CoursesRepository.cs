using Microsoft.EntityFrameworkCore;
using Platform.Application.DTOs;
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
        List<Course> matchedCourses = [];
        List<UserCourse> userCourses =
         await _dbContext.UserCourses
            .Where(uc => uc.UserId == id)
            .ToListAsync();
        // aq null checki sachiroa?
        if (userCourses == null || userCourses.Count == 0)
        {
            return new CourseResponse
            {
                courses = [],
                result = CourseSearchResultEnum.UserNotFound
            };
        }

        foreach (UserCourse userCourse in userCourses)
        {
            var course = await _dbContext.Courses.FindAsync(userCourse.CourseId);
            if (course != null)
            {
                matchedCourses.Add(course);
            }
        }
        return new CourseResponse
        {
            courses = matchedCourses,
            result = CourseSearchResultEnum.Success
        };
    }

    public async Task<CourseResponse> EnrollInCourse(CourseToEnrollDTO courseToEnroll)
    {
        var courseFound = await _dbContext.Courses.FindAsync(courseToEnroll.CourseId);

        if (courseFound == null)
        {
            return new CourseResponse
            {
                result = CourseSearchResultEnum.NotFound
            };
        }
        var userFound = await _dbContext.Users.FindAsync(courseToEnroll.UserId);
        if (userFound == null)
        {
            return new CourseResponse
            {
                result = CourseSearchResultEnum.UserNotFound
            };
        }

        await _dbContext.UserCourses.AddAsync(
            new UserCourse
            {
                UserId = courseToEnroll.UserId,
                CourseId = courseToEnroll.CourseId,
            }
        );

        await _dbContext.SaveChangesAsync();

        return new CourseResponse
        {
            result = CourseSearchResultEnum.Success
        };
    }

    public async Task<CourseResponse> AddCourse(AddCourseDTO courseData)
    {
        var userFound = await _dbContext.Users.FindAsync(courseData.AuthorId);

        // aq amis checki sachiroa?
        if (userFound == null)
        {
            return new CourseResponse
            {
                result = CourseSearchResultEnum.UserNotFound
            };
        }

        if (userFound.UserType != (int)UserTypeEnum.Teacher)
        {
            return new CourseResponse
            {
                result = CourseSearchResultEnum.UserNotAuthorized
            };
        }

        var course = new Course
        {
            AuthorUserId = courseData.AuthorId,
            CourseDescription = courseData.CourseDescription,
            CourseTitle = courseData.CourseTitle
        };
        //es kursi bazidanr ogor unda wamovigo ver vxvdebi
        await _dbContext.Courses.AddAsync(course);

        await _dbContext.SaveChangesAsync();



        return new CourseResponse
        {
            course = course,
            result = CourseSearchResultEnum.Success
        };

    }


}