using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.Models
{
    public class Auth
    {
        [Key] public int id { get; set; }

        [ForeignKey("User")] public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = [];
        public byte[] PasswordSalt { get; set; } = [];
    }
}