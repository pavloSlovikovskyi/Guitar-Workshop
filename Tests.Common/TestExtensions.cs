using Application.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Tests.Common;

public static class TestExtensions
{
    public const string TestUserIdHeader = "X-Test-User-Id";
    public const string TestRolesHeader = "X-Test-Roles";
    public const string SmartAuthScheme = "Smart";

    public static WebApplicationFactory<Program> WithWebHostBuilderMock(this IntegrationTestWebFactory factory)
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IConfigureOptions<AuthenticationOptions>>();

                services.AddAuthentication(SmartAuthScheme)
                    .AddPolicyScheme(SmartAuthScheme, SmartAuthScheme, options =>
                    {
                        options.ForwardDefaultSelector = context =>
                        {
                            var authorization = context.Request.Headers.Authorization.ToString();
                            return authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                                ? JwtBearerDefaults.AuthenticationScheme
                                : TestAuthHandler.SchemeName;
                        };
                    })
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        TestAuthHandler.SchemeName,
                        _ => { })
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);

                services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                    .Configure<IConfiguration>((options, configuration) =>
                    {
                        var jwtSection = configuration.GetSection("Jwt");
                        var jwtKey = jwtSection["Key"]
                                     ?? throw new InvalidOperationException("JWT Key is not configured.");

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSection["Issuer"],
                            ValidAudience = jwtSection["Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                            ClockSkew = TimeSpan.Zero
                        };
                    });

                services.AddSingleton<IConfigureOptions<AuthenticationOptions>, TestAuthenticationOptions>();
            });
        });
    }

    public static HttpClient CreateMasterClient(this WebApplicationFactory<Program> factory)
    {
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        client.DefaultRequestHeaders.Add(TestUserIdHeader, Guid.NewGuid().ToString());
        client.DefaultRequestHeaders.Add(TestRolesHeader, AppRoles.Master);
        return client;
    }

    public static HttpClient CreateCustomerClient(
        this WebApplicationFactory<Program> factory,
        string identityUserId)
    {
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        client.DefaultRequestHeaders.Add(TestUserIdHeader, identityUserId);
        client.DefaultRequestHeaders.Add(TestRolesHeader, AppRoles.Customer);
        return client;
    }

    public static async Task<T> ToResponseModel<T>(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(content)
               ?? throw new ArgumentException("Response content cannot be null");
    }
}

internal sealed class TestAuthenticationOptions : IConfigureOptions<AuthenticationOptions>
{
    public void Configure(AuthenticationOptions options)
    {
        options.DefaultAuthenticateScheme = TestExtensions.SmartAuthScheme;
        options.DefaultChallengeScheme = TestExtensions.SmartAuthScheme;
    }
}

public class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "Test";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(TestExtensions.TestRolesHeader))
            return Task.FromResult(AuthenticateResult.NoResult());

        var userId = Request.Headers[TestExtensions.TestUserIdHeader].FirstOrDefault()
                     ?? Guid.NewGuid().ToString();
        var rolesHeader = Request.Headers[TestExtensions.TestRolesHeader].FirstOrDefault()
                          ?? AppRoles.Master;

        var roleClaims = rolesHeader
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(role => new Claim(ClaimTypes.Role, role));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, $"{userId}@test.local")
        };
        claims.AddRange(roleClaims);

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
