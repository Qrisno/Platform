
using Platform.Application.Enums;
using Platform.Domain.Entities.Models;

namespace Platform.Application.Models;


public class CourseResponse
{
    public List<Course> courses { get; set; } = [];
    public Course? course { get; set; }

    public CourseSearchResultEnum result { get; set; }
}