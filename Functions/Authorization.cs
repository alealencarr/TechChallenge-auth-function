using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Web;
using TechChallenge_auth_function.Repositories.Customer;
using TechChallenge_auth_function.Services.Token;

namespace TechChallenge_auth_function.Functions;

public class Authorization
{
    private readonly ILogger _logger;
    private readonly ITokenService _tokenService;
    private readonly ICustomerRepository _customerRepository;
    public Authorization(ILoggerFactory logger, ITokenService tokenService, ICustomerRepository customerRepository)
    {
        _logger = logger.CreateLogger<Authorization>();
        _tokenService = tokenService;
        _customerRepository = customerRepository;
    }

    [Function("Authorization")]
    public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "auth")] HttpRequestData req)
    {
        _logger.LogInformation("Função de identificação de cliente foi acionada.");

        var query = HttpUtility.ParseQueryString(req.Url.Query);
        var cpf = query["document"];

        string token;

        if (!string.IsNullOrWhiteSpace(cpf))
        {
            var customer = await _customerRepository.GetCustomerByCpf(cpf);
            token = _tokenService.GenerateCustomerToken(customer);
        }
        else
        {
            token = _tokenService.GenerateGuestToken();
        }
        var tokenResponse = new
        {
            access_token = token, 
            token_type = "Bearer",  
            expires_in = 3600,  
            scope = "read write"  
        };

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(tokenResponse);
        return response;  
    }
}
