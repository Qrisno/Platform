using Platform.Application.Models;
using Platform.Domain.Entities.Models;

namespace Platform.Application.Repos;

public interface ICoursesRepository
{
    Task<CourseResponse> GetCoursesByAuthor(int id);

    Task<CourseResponse> GetCoursesByStudent(int id);
}
