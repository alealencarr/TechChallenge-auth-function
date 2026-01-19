namespace TechChallenge_auth_function.Domain.Entities
{
    public class User
    {
        public User(string clientId, string clientSecret, string name)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            ClientId = clientId;
            ClientSecret = clientSecret;
            Name = name;
        }

        protected User()
        {

        }

        public DateTime CreatedAt { get; private set; }

        public Guid Id { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

    }
}
