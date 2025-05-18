namespace Platform.Application.DTOs
{
    public class AddCourseDTO
    {
        public int AuthorId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public string CourseDescription { get; set; } = string.Empty;
    }
}