namespace TechChallenge_auth_function.Repositories.Customer
{
    public interface ICustomerRepository
    {
        Task Create(string cpf, string name, string mail);
        Task<Entities.Customer?> GetCustomerByCpf(string cpf);
    }
}
