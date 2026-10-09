using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Entity;

namespace AuthenticationService.Service
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UserEntity user)
        {
            var key = _configuration["Jwt:Key"]!;
            var issuer = _configuration["Jwt:Issuer"]!;
            var audience = _configuration["Jwt:Audience"]!;
            var expiryMinutes = int.Parse(
                _configuration["Jwt:ExpiryMinutes"]!);

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                new Claim(
                    ClaimTypes.Email,
                    user.Email),

                new Claim(
                    ClaimTypes.Name,
                    user.FirstName)
            };

            return CreateToken(
                claims,
                key,
                issuer,
                audience,
                expiryMinutes);
        }

        public string GenerateResetToken(UserEntity user)
        {
            var key = _configuration["Jwt:Key"]!;
            var issuer = _configuration["Jwt:Issuer"]!;
            var audience = _configuration["Jwt:Audience"]!;

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                new Claim(
                    ClaimTypes.Email,
                    user.Email),

                new Claim(
                    "purpose",
                    "password-reset")
            };

            return CreateToken(
                claims,
                key,
                issuer,
                audience,
                10);
        }

        public int ValidateResetToken(string token)
        {
            var key = _configuration["Jwt:Key"]!;
            var issuer = _configuration["Jwt:Issuer"]!;
            var audience = _configuration["Jwt:Audience"]!;

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = issuer,
                    ValidAudience = audience,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(key))
                };

            var principal = tokenHandler.ValidateToken(
                token,
                validationParameters,
                out _);

            var purpose =
                principal.FindFirst("purpose")?.Value;

            if (purpose != "password-reset")
            {
                throw new SecurityTokenException(
                    "Invalid reset token.");
            }

            var userId =
                principal.FindFirst(
                    ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userId, out var id))
            {
                throw new SecurityTokenException(
                    "Invalid user.");
            }

            return id;
        }

        private string CreateToken(
            IEnumerable<Claim> claims,
            string key,
            string issuer,
            string audience,
            int expiryMinutes)
        {
            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key));

            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    expiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}