

using System.ComponentModel.DataAnnotations;

namespace Platform.Domain.Entities.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public int UserType { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int[] Courses { get; set; } = [];
    }
}