namespace Platform.Domain.Entities.Models
{
    public class UserCourse
    {
        public int UserCourseId { get; set; }
        public int UserId { get; set; }

        public int CourseId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public string? Progress { get; set; }
    }
}