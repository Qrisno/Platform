
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [ForeignKey("User")]
        public int AuthorUserId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public string CourseDescription { get; set; } = string.Empty;
        public string CourseLength { get; set; } = string.Empty;
    }
}