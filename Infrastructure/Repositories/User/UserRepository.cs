using Microsoft.EntityFrameworkCore;
using TechChallenge_auth_function.Infrastructure;

namespace TechChallenge_auth_function.Infrastructure.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Domain.Entities.User?> ValidateAutentication(string clientId, string clientSecret)
        {
            return await _dbContext.User.AsNoTracking().Where(x => x.ClientId.Equals(clientId) && x.ClientSecret.Equals(clientSecret)).FirstOrDefaultAsync();
        }
    }
}
