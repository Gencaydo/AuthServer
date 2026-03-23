namespace AuthServer.Core.Dtos;

public class UserLoginDto
{
    public string? UserId { get; set; }
    public string? LoginProvider { get; set; }
    public string? ProviderKey { get; set; }
    public string? ProviderDisplayName { get; set; }
}
