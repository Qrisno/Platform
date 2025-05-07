

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.Models
{
    public class UserCourse
    {
        public int UserId { get; set; }

        public int CourseId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public string? Progress { get; set; }

    }
}