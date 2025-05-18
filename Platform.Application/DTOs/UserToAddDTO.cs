namespace Platform.Application.DTOs
{
    public class UserToAddDTO
    {
        public int UserType { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}