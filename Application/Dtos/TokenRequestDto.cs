namespace TechChallenge_auth_function.Application.Dtos
{
    public record TokenRequestDto
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
