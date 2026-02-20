using Microsoft.AspNetCore.Identity;

namespace RealTimeChat.Models;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
}