using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Web;
using TechChallenge_auth_function.Application.Gateways;
using TechChallenge_auth_function.Application.Services.Token;

namespace TechChallenge_auth_function.Application.Functions;

public class Authorization
{
    private readonly ILogger _logger;
    private readonly ITokenService _tokenService;
    private readonly ICustomerHttpService _customerHttpService;
    public Authorization(ILoggerFactory logger, ITokenService tokenService, ICustomerHttpService customerHttpService)
    {
        _logger = logger.CreateLogger<Authorization>();
        _tokenService = tokenService;
        _customerHttpService = customerHttpService;
    }

    [Function("Authorization")]
    public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "auth")] HttpRequestData req)
    {
        _logger.LogInformation("Função de identificação de cliente foi acionada.");

        var query = HttpUtility.ParseQueryString(req.Url.Query);
        var cpf = query["document"];

        string token;

        //if (!string.IsNullOrWhiteSpace(cpf))
        //{
        //    var customer = await _customerHttpService.GetCustomerByCpf(cpf);

        //    if (customer is null)
        //    {
        //        var message = "Cliente não encontrado com esse CPF.";

        //        var errorCpf = req.CreateResponse(HttpStatusCode.BadRequest);

        //        await errorCpf.WriteAsJsonAsync(new
        //        {
        //            data = (object?)null,
        //            messages = new List<string> { message }
        //        });
        //        return errorCpf;
        //    }

        //    token = _tokenService.GenerateCustomerToken(customer);
        //}
        //else
        //{
            token = _tokenService.GenerateGuestToken();
        //}

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
