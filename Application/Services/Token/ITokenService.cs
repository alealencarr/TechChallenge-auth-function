using TechChallenge_auth_function.Domain.Entities;

namespace TechChallenge_auth_function.Application.Services.Token
{
    public interface ITokenService
    {
        /// <summary>
        /// Gera um token para um cliente identificado (via CPF).
        /// </summary>
        string GenerateCustomerToken(Customer customer);

        /// <summary>
        /// Gera um token para um cliente anônimo (convidado).
        /// </summary>
        string GenerateGuestToken();
        string GenerateUserToken(User user);
    }
}
