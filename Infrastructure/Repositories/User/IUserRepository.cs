namespace TechChallenge_auth_function.Infrastructure.Repositories.User
{
    public interface IUserRepository
    {
        Task<Domain.Entities.User?> ValidateAutentication(string clientId, string clientSecret);
    }
}
