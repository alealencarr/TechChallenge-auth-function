using Microsoft.EntityFrameworkCore;
using TechChallenge_auth_function.Infrastructure;

namespace TechChallenge_auth_function.Repositories.Customer
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;
        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(string cpf, string name, string mail)
        {
            var customer = new Entities.Customer(cpf.Replace(".", "").Replace("-", ""), name, mail);
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Entities.Customer?> GetCustomerByCpf(string cpf)
        {            
            var customer = await _dbContext.Customers.AsNoTracking().Where(x => x.Cpf.Replace(".", "").Replace("-", "") == cpf.Replace(".", "").Replace("-", "")).FirstOrDefaultAsync();

            return customer;

        }
    }
}
