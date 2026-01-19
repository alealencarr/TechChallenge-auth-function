using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using TechChallenge_auth_function.Application.Services.Token;

namespace TechChallenge_auth_function.Application.Externals;
public class TokenAuthenticationHandler(
    ITokenService tokenService )
    : DelegatingHandler
{

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = tokenService.GenerateGuestToken();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}

