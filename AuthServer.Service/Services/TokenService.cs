using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthServer.Core.Configuration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configuration;

namespace AuthServer.Service.Services;

public class TokenService : ITokenService
{

    private readonly UserManager<UserApp> _userManager;

    private readonly CustomTokenOption _tokenOption;

    public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOption> options)
    {
        _tokenOption = options.Value;
        _userManager = userManager;
    }

    private string CreateRefreshToken()
    {
        // Generate a random 32-byte refresh token
        var numberByte = new byte[32];
        using var rnd = RandomNumberGenerator.Create();
        rnd.GetBytes(numberByte);
        var refreshToken = Convert.ToBase64String(numberByte);

        // Encrypt the refresh token
        return EncryptToken(refreshToken);
    }

    //Buradaki claimler payloadda gözükecek.    
    private async Task<IEnumerable<Claim>> GetClaims(UserApp userApp, List<string> audiences)
    {
        var userRoles = await _userManager.GetRolesAsync(userApp);
        var userList = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userApp.Id),
            new Claim(ClaimTypes.Email, userApp.Email),
            new Claim(ClaimTypes.Name, userApp.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x))); //Audiences.
        userList.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

        return userList;
    }

    private IEnumerable<Claim> GetClaimsByClient(Client client)
    {
        var claims = new List<Claim>();
        claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, client.ClientId));
        return claims;
    }


    public TokenDto CreateToken(UserApp userApp)
    {
        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOption.AccessTokenExpiration);
        var refreshTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOption.RefreshTokenExpiration);

        var securityKey = SignInService.GetSymmetricSecurityKey(_tokenOption.SecurityKey);

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _tokenOption.Issuer,
            expires: accessTokenExpiration,
            notBefore: DateTime.UtcNow,
            claims: GetClaims(userApp, _tokenOption.Audience).Result,
            signingCredentials: signingCredentials
        );

        var handler = new JwtSecurityTokenHandler();

        var token = handler.WriteToken(jwtSecurityToken);

        var tokenDto = new TokenDto()
        {
            AccessToken = EncryptToken(token),
            RefreshToken = CreateRefreshToken(),
            AccessTokenExpiration = accessTokenExpiration,
            RefreshTokenExpiration = refreshTokenExpiration
        };

        return tokenDto;
    }

    public ClientTokenDto CreateTokenByClient(Client client)
    {
        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOption.AccessTokenExpiration);
        var securityKey = SignInService.GetSymmetricSecurityKey(_tokenOption.SecurityKey);

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _tokenOption.Issuer,
            expires: accessTokenExpiration,
            notBefore: DateTime.UtcNow,
            claims: GetClaimsByClient(client),
            signingCredentials: signingCredentials
        );

        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwtSecurityToken);
        var clientTokenDto = new ClientTokenDto()
        {
            AccessToken = token,
            AccessTokenExpiration = accessTokenExpiration,
        };

        return clientTokenDto;
    }

    private string EncryptToken(string token)
    {
        var key = Encoding.UTF8.GetBytes("YourStrongEncryptionKey123"); // 32 characters for AES-256
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();
        var iv = Convert.ToBase64String(aes.IV);

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var tokenBytes = Encoding.UTF8.GetBytes(token);
        var encryptedBytes = encryptor.TransformFinalBlock(tokenBytes, 0, tokenBytes.Length);

        return $"{iv}:{Convert.ToBase64String(encryptedBytes)}";
    }
}

