using Platform.Application.DTOs;
using Platform.Application.Models;

namespace Platform.Application.Repos;

public interface ICoursesRepository
{
    Task<CourseResponse> GetCoursesByAuthor(int id);

    Task<CourseResponse> GetCoursesByStudent(int id);
    Task<CourseResponse> AddCourse(AddCourseDTO course);
    Task<CourseResponse> EnrollInCourse(CourseToEnrollDTO course);
}
