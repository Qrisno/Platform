// es layer ra sachiroa pirdapir rom davudzaxo ra uchirs?

using Platform.Application.DTOs;
using Platform.Application.Models;
using Platform.Application.Repos;

public class CourseService(ICoursesRepository repo)
{
    public async Task<CourseResponse> GetCoursesByAuthor(int id)
    {
        return await repo.GetCoursesByAuthor(id);
    }

    public async Task<CourseResponse> GetCoursesByStudent(int id)
    {
        return await repo.GetCoursesByStudent(id);
    }

    public async Task<CourseResponse> AddCourse(AddCourseDTO course)
    {
        return await repo.AddCourse(course);
    }

    public async Task<CourseResponse> Enroll(CourseToEnrollDTO course)
    {
        return await repo.EnrollInCourse(course);
    }
}