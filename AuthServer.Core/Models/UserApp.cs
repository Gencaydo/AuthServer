using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Models;

public class UserApp : IdentityUser
{
    public string? FistName { get; set; } = null;
    public string? LastName { get; set; } = null;
    public string? MobilePhoneNumber { get; set; } = null;
}

