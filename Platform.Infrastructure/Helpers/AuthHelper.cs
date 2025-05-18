using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Platform.Infrastructure.Helpers
{
    public class AuthHelper
    {
        private readonly IConfiguration _config;

        public AuthHelper(IConfiguration config)
        {
            _config = config;
        }

        public byte[] GetPasswordHash(string password, byte[] salt)
        {
            string tokenKeyFromSettings = _config.GetSection("AppSettings:TokenKey").Value ?? string.Empty;
            string passwordSaltPlusTokenKey = tokenKeyFromSettings + Convert.ToBase64String(salt);
            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password,
                Encoding.ASCII.GetBytes(passwordSaltPlusTokenKey),
                KeyDerivationPrf.HMACSHA256,
                1000,
                256 / 8
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
            Claim[] claims =
            [
                new("userId", userId.ToString())
            ];

            string tokenKeyFromSettings = _config.GetSection("TokenKey").Value ?? string.Empty;

            SymmetricSecurityKey tokenKey = new(
                Encoding.UTF8.GetBytes(tokenKeyFromSettings)
            );

            SigningCredentials signingCredentials = new(tokenKey, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor securityTokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signingCredentials,
                Expires = DateTime.Now.AddDays(1)
            };

            JwtSecurityTokenHandler handler = new();
            SecurityToken token = handler.CreateToken(securityTokenDescriptor);

            return handler.WriteToken(token);
        }
    }
}