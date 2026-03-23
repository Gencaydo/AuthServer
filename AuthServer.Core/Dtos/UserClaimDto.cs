namespace AuthServer.Core.Dtos;

public class UserClaimDto
{
    public string? UserId { get; set; }
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }
}
