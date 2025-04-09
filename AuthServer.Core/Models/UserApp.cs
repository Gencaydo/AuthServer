using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Models;

public class UserApp : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MobilePhoneNumber { get; set; }
}

