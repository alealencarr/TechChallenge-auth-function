using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Web;
using TechChallenge_auth_function.Repositories.Customer;

namespace TechChallenge_auth_function.Functions;

public class GetByCpf
{
    private readonly ILogger _logger;
    private readonly ICustomerRepository _customerRepository;
    public GetByCpf(ILoggerFactory logger, ICustomerRepository customerRepository)
    {
        _logger = logger.CreateLogger<GetByCpf>();
        _customerRepository = customerRepository;
    }
    [Function("GetByCpf")]
    public async Task<HttpResponseData> Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getbycpf")] HttpRequestData req)
    {
        _logger.LogInformation("Função de consulta de cliente acionada.");

        try
        {
            var query = HttpUtility.ParseQueryString(req.Url.Query);
            var cpf = query["document"];

            if (string.IsNullOrWhiteSpace(cpf))
            {
                var message = "É necessário informar o CPF.";
                var error = req.CreateResponse(HttpStatusCode.BadRequest);

                await error.WriteAsJsonAsync(new
                {
                    data = (object?)null,
                    messages = new List<string> { message }
                });
                return error;
            }

            var customer = await _customerRepository.GetCustomerByCpf(cpf);

            if (customer == null)
            {
                var message = "Cliente não encontrado.";

                var error = req.CreateResponse(HttpStatusCode.BadRequest);

                await error.WriteAsJsonAsync(new
                {
                    data = (object?)null,
                    messages = new List<string> { message }
                });
                return error;
            }

            _logger.LogInformation($"Cliente com CPF {cpf} encontrado com sucesso.");

            var response = req.CreateResponse(HttpStatusCode.OK);

            var customerResponse = new
            {
                customer.Id,
                customer.Cpf,
                customer.Mail,
                customer.Name
            };

            var ret = new
            {
                data = customerResponse,
                messages = new List<string> { "Cliente encontrado com sucesso." }
            };
            await response.WriteAsJsonAsync(ret);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar cliente.");
            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}

