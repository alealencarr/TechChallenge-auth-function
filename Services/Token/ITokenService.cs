namespace TechChallenge_auth_function.Services.Token
{
    public interface ITokenService
    {
        /// <summary>
        /// Gera um token para um cliente identificado (via CPF).
        /// </summary>
        string GenerateCustomerToken(Entities.Customer customer);

        /// <summary>
        /// Gera um token para um cliente anônimo (convidado).
        /// </summary>
        string GenerateGuestToken();
    }
}
