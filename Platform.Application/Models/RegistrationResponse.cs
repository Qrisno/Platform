using Platform.Domain.Entities.Models;

namespace Platform.Application.Models
{
    public class RegistrationResponse : AuthResponse
    {
        public User? User { get; set; }
    }
}