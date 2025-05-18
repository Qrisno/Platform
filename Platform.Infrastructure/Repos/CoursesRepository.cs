using Microsoft.EntityFrameworkCore;
using Platform.Application.DTOs;
using Platform.Application.Enums;
using Platform.Application.Models;
using Platform.Application.Repos;
using Platform.Domain.Entities.Models;
using Platform.Infrastructure.Interfaces;

namespace Platform.Infrastructure.Repos
{
    public class CoursesRepository(IPlatformDbContext dbContext) : ICoursesRepository
    {
        public async Task<CourseResponse> GetCoursesByAuthor(int id)
        {
            List<Course> filteredCourses =
                await dbContext.Courses
                    .Where(c => c.AuthorUserId == id)
                    .ToListAsync();

            if (filteredCourses.Count == 0)
            {
                return new CourseResponse { courses = [], result = CourseSearchResultEnum.NotFound };
            }

            return new CourseResponse { courses = filteredCourses, result = CourseSearchResultEnum.Success };
        }

        public async Task<CourseResponse> GetCoursesByStudent(int id)
        {
            List<Course> matchedCourses = [];
            List<UserCourse> userCourses =
                await dbContext.UserCourses
                    .Where(uc => uc.UserId == id)
                    .ToListAsync();
            // aq null checki sachiroa?
            if (userCourses == null || userCourses.Count == 0)
            {
                return new CourseResponse { courses = [], result = CourseSearchResultEnum.UserNotFound };
            }

            foreach (UserCourse userCourse in userCourses)
            {
                Course? course = await dbContext.Courses.FindAsync(userCourse.CourseId);
                if (course != null)
                {
                    matchedCourses.Add(course);
                }
            }

            return new CourseResponse { courses = matchedCourses, result = CourseSearchResultEnum.Success };
        }

        public async Task<CourseResponse> EnrollInCourse(CourseToEnrollDTO courseToEnroll)
        {
            Course? courseFound = await dbContext.Courses.FindAsync(courseToEnroll.CourseId);

            if (courseFound == null)
            {
                return new CourseResponse { result = CourseSearchResultEnum.NotFound };
            }

            User? userFound = await dbContext.Users.FindAsync(courseToEnroll.UserId);
            if (userFound == null)
            {
                return new CourseResponse { result = CourseSearchResultEnum.UserNotFound };
            }

            await dbContext.UserCourses.AddAsync(
                new UserCourse { UserId = courseToEnroll.UserId, CourseId = courseToEnroll.CourseId }
            );

            await dbContext.SaveChangesAsync();

            return new CourseResponse { result = CourseSearchResultEnum.Success };
        }

        public async Task<CourseResponse> AddCourse(AddCourseDTO courseData)
        {
            User? userFound = await dbContext.Users.FindAsync(courseData.AuthorId);

            // aq amis checki sachiroa?
            if (userFound == null)
            {
                return new CourseResponse { result = CourseSearchResultEnum.UserNotFound };
            }

            if (userFound.UserType != (int)UserTypeEnum.Teacher)
            {
                return new CourseResponse { result = CourseSearchResultEnum.UserNotAuthorized };
            }

            Course course = new()
            {
                AuthorUserId = courseData.AuthorId,
                CourseDescription = courseData.CourseDescription,
                CourseTitle = courseData.CourseTitle
            };
            //es kursi bazidanr ogor unda wamovigo ver vxvdebi
            await dbContext.Courses.AddAsync(course);

            await dbContext.SaveChangesAsync();


            return new CourseResponse { course = course, result = CourseSearchResultEnum.Success };
        }
    }
}