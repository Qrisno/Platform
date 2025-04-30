

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.Models
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public string LessonDescription { get; set; } = string.Empty;
        public string LessonLength { get; set; } = string.Empty;

    }
}