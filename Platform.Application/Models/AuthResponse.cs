
using Platform.Application.Enums;

namespace Platform.Application.Models;

public class AuthResponse
{
    public AuthStatusEnum AuthStatus { get; set; }
    public string? ReasonText { get; set; }
}