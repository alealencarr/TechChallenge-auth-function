using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using TechChallenge_auth_function.Application.Dtos;
using TechChallenge_auth_function.Application.Services.Token;
using TechChallenge_auth_function.Infrastructure.Repositories.User;

namespace TechChallenge_auth_function.Application.Functions;

public class Token
{
    private readonly ILogger _logger;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    public Token(ILoggerFactory logger, ITokenService tokenService, IUserRepository userRepository)
    {
        _logger = logger.CreateLogger<Token>();
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    [Function("Token")]
    public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "token")] HttpRequestData req)
    {
        _logger.LogInformation("Função de geração de Token foi acionada.");

        try
        {
            var requestDto = await req.ReadFromJsonAsync<TokenRequestDto>();

            if (requestDto == null || string.IsNullOrWhiteSpace(requestDto.ClientId) || string.IsNullOrWhiteSpace(requestDto.ClientSecret))
            {
                var message = "É necessário informar o Client Id e o Client Secret.";

                var errorCpf = req.CreateResponse(HttpStatusCode.BadRequest);

                await errorCpf.WriteAsJsonAsync(new
                {
                    data = (object?)null,
                    messages = new List<string> { message }
                });
                return errorCpf;
            }

            var user = await _userRepository.ValidateAutentication(requestDto.ClientId, requestDto.ClientSecret);

            if (user is null)
            {
                var message = "Informações não encontradas com base neste Client Id e Client Secret.";

                var errorCpf = req.CreateResponse(HttpStatusCode.BadRequest);

                await errorCpf.WriteAsJsonAsync(new
                {
                    data = (object?)null,
                    messages = new List<string> { message }
                });
                return errorCpf;

            }

            var token = _tokenService.GenerateUserToken(user);

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar token para o usuário.");
            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
