namespace AuthServer.Core.Dtos;

public class CreateUserDto
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? FistName { get; set; } = null;
    public string? LastName { get; set; } = null;
    public string? MobilePhoneNumber { get; set; } = null;
}
