using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using VictorKrogh.NET.Authentication.Handlers;

namespace VictorKrogh.NET.Authentication.DependencyInjection;

public static class AuthenticationAuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddJwtAuthentication(this AuthenticationBuilder authenticationBuilder)
    {
        authenticationBuilder.Services.AddTransient<IJwtTokenHandler, JwtTokenHandler>();
        authenticationBuilder.Services.AddTransient<IJwtRefreshTokenHandler, JwtRefreshTokenHandler>();
        authenticationBuilder.Services.AddTransient<IJwtAuthenticationService, JwtAuthenticationService>();

        return authenticationBuilder;
    }
}
