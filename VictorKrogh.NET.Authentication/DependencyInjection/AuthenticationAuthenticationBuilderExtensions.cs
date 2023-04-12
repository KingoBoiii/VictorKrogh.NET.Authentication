using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VictorKrogh.NET.Authentication.Handlers;

namespace VictorKrogh.NET.Authentication.DependencyInjection;

public static class AuthenticationAuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddJwtAuthentication(this AuthenticationBuilder authenticationBuilder, IJwtAuthenticationOptions jwtAuthenticationOptions)
    {
        authenticationBuilder.Services.AddTransient<IJwtTokenHandler, JwtTokenHandler>(x => new JwtTokenHandler(x.GetRequiredService<ILogger<JwtTokenHandler>>(), jwtAuthenticationOptions));
        authenticationBuilder.Services.AddTransient<IJwtRefreshTokenHandler, JwtRefreshTokenHandler>(x => new JwtRefreshTokenHandler(jwtAuthenticationOptions));
        authenticationBuilder.Services.AddTransient<IJwtAuthenticationService, JwtAuthenticationService>();

        return authenticationBuilder;
    }
}
