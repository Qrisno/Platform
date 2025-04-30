using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Platform.Infrastructure.Helpers;

public class AuthHelper
{
    private IConfiguration _config;
    public AuthHelper(IConfiguration config)
    {
        _config = config;
    }

    public byte[] GetPasswordHash(string password, byte[] salt)
    {
        var tokenKeyFromSettings = _config.GetSection("AppSettings:TokenKey").Value ?? string.Empty;
        string passwordSaltPlusTokenKey = tokenKeyFromSettings + Convert.ToBase64String(salt);
        byte[] passwordHash = KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(passwordSaltPlusTokenKey),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 1000,
            numBytesRequested: 256 / 8

        );
        return passwordHash;
    }

    public byte[] GetPasswordSalt()
    {
        byte[] salt = new byte[128 / 8];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(salt);
        }

        return salt;
    }


    public string GetToken(int userId)
    {
        Claim[] claims = [
            new Claim("userId", userId.ToString())
        ];

        var tokenKeyFromSettings = _config.GetSection("TokenKey").Value ?? string.Empty;

        SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(tokenKeyFromSettings)
        );

        SigningCredentials signingCredentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

        SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = signingCredentials,
            Expires = DateTime.Now.AddDays(1)
        };

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        SecurityToken token = handler.CreateToken(securityTokenDescriptor);

        return handler.WriteToken(token);
    }
}