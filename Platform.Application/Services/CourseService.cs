// es layer ra sachiroa pirdapir rom davudzaxo ra uchirs?

using Platform.Application.DTOs;
using Platform.Application.Models;
using Platform.Application.Repos;

public class CourseService
{
    private ICoursesRepository _repo;
    public CourseService(ICoursesRepository repo)
    {
        _repo = repo;
    }
    public async Task<CourseResponse> GetCoursesByAuthor(int id)
    {
        return await _repo.GetCoursesByAuthor(id);
    }

    public async Task<CourseResponse> GetCoursesByStudent(int id)
    {
        return await _repo.GetCoursesByStudent(id);
    }

    public async Task<CourseResponse> AddCourse(AddCourseDTO course)
    {
        return await _repo.AddCourse(course);
    }
}