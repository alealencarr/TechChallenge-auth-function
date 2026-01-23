using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TechChallenge_auth_function.Application.Externals;
using TechChallenge_auth_function.Application.Gateways;
using TechChallenge_auth_function.Application.Services.Token;
using TechChallenge_auth_function.Infrastructure;
using TechChallenge_auth_function.Infrastructure.HttpService;
using TechChallenge_auth_function.Infrastructure.Repositories.User;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerHttpService, CustomerHttpService>();
builder.Services.AddTransient<TokenAuthenticationHandler>();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("Default"), options =>
    {
        options.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        );
    });
});

var baseUrl = builder.Configuration[$"CustomersHttpClient:BaseUrl"];

builder.Services.AddHttpClient("CustomersHttpClient", client =>
{
    if (!string.IsNullOrEmpty(baseUrl))
        client.BaseAddress = new Uri(baseUrl);

    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.AddHttpMessageHandler<TokenAuthenticationHandler>();


builder.Build().Run();
