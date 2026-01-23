using TechChallenge_auth_function.Domain.Entities;

namespace TechChallenge_auth_function.Application.Gateways
{
    public interface ICustomerHttpService
    {
        Task<Customer?> GetCustomerByCpf(string cpf);
    }
}
