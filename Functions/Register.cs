using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using TechChallenge_auth_function.Dtos;
using TechChallenge_auth_function.Repositories.Customer;
using TechChallenge_auth_function.Services.Token;

namespace TechChallenge_auth_function.Functions;

public class Register
{
    private readonly ILogger _logger;
    private readonly ICustomerRepository _customerRepository;
    public Register(ILoggerFactory logger, ICustomerRepository customerRepository)
    {
        _logger = logger.CreateLogger<Register>();
        _customerRepository = customerRepository;
    }
    [Function("Register")]
    public async Task<HttpResponseData> Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "register")] HttpRequestData req)
    {
        _logger.LogInformation("Função de registro de cliente acionada.");

        try
        {
            var customerDto = await req.ReadFromJsonAsync<CustomerRequestDto>();

            if (customerDto == null || string.IsNullOrWhiteSpace(customerDto.Cpf))
            {
                var message = "É necessário informar o CPF.";
               
                var errorCpf = req.CreateResponse(HttpStatusCode.BadRequest);

                await errorCpf.WriteAsJsonAsync(new
                {
                    data = (object?)null,
                    messages = new List<string> { message }
                });
                return errorCpf;
            }

            var existingCustomer = await _customerRepository.GetCustomerByCpf(customerDto.Cpf);
            if (existingCustomer != null)
            {
                var responseReq = req.CreateResponse(HttpStatusCode.Conflict);
                var message = "Cliente com este CPF já cadastrado.";
          
                await responseReq.WriteAsJsonAsync(new
                {
                    data = (object?)null,
                    messages = new List<string> { message }
                });
                return responseReq;
            }

            await _customerRepository.Create(customerDto.Cpf, customerDto.Name, customerDto.Mail);

            _logger.LogInformation($"Cliente com CPF {customerDto.Cpf} registrado com sucesso.");

            var response = req.CreateResponse(HttpStatusCode.Created);
            
            var ret = new
            {
                data = customerDto,
                messages = new List<string> { "Cliente cadastrado com sucesso." }
            };
            await response.WriteAsJsonAsync(ret);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar cliente.");
            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}

