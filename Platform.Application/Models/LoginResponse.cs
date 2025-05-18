namespace Platform.Application.Models
{
    public class LoginResponse : AuthResponse
    {
        public string? Token { get; set; }
    }
}