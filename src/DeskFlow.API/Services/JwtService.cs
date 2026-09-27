using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DeskFlow.API.Services
{
    public class JwtService : IJwtService
    {
      private readonly IConfiguration _configuration;
      private readonly UserManager<ApplicationUser> _userManager;
      public JwtService (IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }  
        public async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id),
                new (JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add (new Claim(ClaimTypes.Role, role));
            }

            var key = _configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("A chave JWT não está configurada.");
            }

            var issuer = _configuration["Jet:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expirationInMinutes = _configuration.GetValue<int>("Jwt: ExpirationInMinutes");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, 
            expires: DateTime.UtcNow.AddMinutes(expirationInMinutes), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}