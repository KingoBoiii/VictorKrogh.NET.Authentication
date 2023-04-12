using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VictorKrogh.NET.Authentication.Handlers;
using VictorKrogh.NET.Authentication.Services;

namespace VictorKrogh.NET.Authentication.DependencyInjection;

public static class AuthenticationServiceCollectionExtensions
{
    public static IServiceCollection AddPasswordHasher(this IServiceCollection services, IPasswordHasherOptions passwordHasherOptions)
    {
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IJwtAuthenticationOptions jwtAuthenticationOptions)
    {
        services.AddTransient<IJwtTokenHandler, JwtTokenHandler>();
        services.AddTransient<IJwtRefreshTokenHandler, JwtRefreshTokenHandler>();
        services.AddTransient<IJwtAuthenticationService, JwtAuthenticationService>();

        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtAuthentication(jwtAuthenticationOptions)
            .AddJwtBearer(x =>
            {
                x.Audience = jwtAuthenticationOptions.Audience;
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthenticationOptions.SigningSecurityKey)),
                    ValidIssuer = jwtAuthenticationOptions.ValidIssuer,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}
