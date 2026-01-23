using System.Net.Http.Json;
using TechChallenge_auth_function.Application.Gateways;
using TechChallenge_auth_function.Domain.Entities;
using TechChallenge_auth_function.Shared.Result;

namespace TechChallenge_auth_function.Infrastructure.HttpService
{
    public class CustomerHttpService : ICustomerHttpService
    {
        private readonly HttpClient _httpClient;

        public CustomerHttpService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("CustomersHttpClient");
        }


        public async Task<Customer?> GetCustomerByCpf(string cpf)
        {
            var result = await _httpClient.GetFromJsonAsync<CommandResult<Customer>>($"/customers/{cpf}");

            return result?.Data;
        }

    }
}
