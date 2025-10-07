using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TechChallenge_auth_function.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
        private readonly string _audience;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!));
            _issuer = _configuration["Jwt:Issuer"]!;
            _audience = _configuration["Jwt:Audience"]!;
        }

        public string GenerateCustomerToken(Entities.Customer customer)
        {
            var claims = new List<Claim>
            {
                new("cpf", customer.Cpf),
                new(ClaimTypes.Name, customer.Name ?? string.Empty),
                new(JwtRegisteredClaimNames.Email, customer.Mail ?? string.Empty),
                new(ClaimTypes.Role, "Customer")
            };

            return GenerateToken(claims, DateTime.UtcNow.AddHours(1));
        }
        public string GenerateGuestToken()
        {
            var claims = new List<Claim>
            {
                // Um ID único para a sessão anônima
                new("guest_id", Guid.NewGuid().ToString()),
                new(ClaimTypes.Role, "Guest")
            };

            return GenerateToken(claims, DateTime.UtcNow.AddHours(1));
        }


        // Método privado para evitar duplicação de código
        private string GenerateToken(IEnumerable<Claim> claims, DateTime expires)
        {

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: expires,
                signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
