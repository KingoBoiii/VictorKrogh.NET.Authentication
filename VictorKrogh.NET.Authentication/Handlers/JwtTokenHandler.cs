using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace VictorKrogh.NET.Authentication.Handlers;

public interface IJwtTokenHandler
{
    Task<string> CreateJwtTokenAsync(IJwtUser jwtUser);
    Task<bool> ValidateTokenAsync(string token);
}

internal sealed class JwtTokenHandler : IJwtTokenHandler
{
    public JwtTokenHandler(ILogger<JwtTokenHandler> logger, IJwtTokenHandlerOptions jwtTokenHandlerOptions)
    {
        Logger = logger;
        JwtTokenHandlerOptions = jwtTokenHandlerOptions;
    }

    private ILogger<JwtTokenHandler> Logger { get; }
    private IJwtTokenHandlerOptions JwtTokenHandlerOptions { get; }

    public Task<string> CreateJwtTokenAsync(IJwtUser jwtUser)
    {
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, jwtUser.Username ?? string.Empty, ClaimValueTypes.String),
            new Claim(ClaimTypes.NameIdentifier, jwtUser.Username ?? string.Empty, ClaimValueTypes.String),
            new Claim(ClaimTypes.AuthenticationMethod, "windows", ClaimValueTypes.String)
        });

        try
        {
            var tokenDescriptor = CreateSecurityTokenDescriptor(claimsIdentity);

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Task.FromResult(tokenHandler.WriteToken(token));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Token handler failed to create Jwt Token");
            return Task.FromResult(string.Empty);
        }
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = CreateTokenValidationParameters();

        try
        {
            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);

            return tokenValidationResult != null && tokenValidationResult.IsValid;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Token handler failed to validate token ({token})");
            return false;
        }
    }

    private SecurityTokenDescriptor CreateSecurityTokenDescriptor(ClaimsIdentity claimsIdentity)
    {
        return new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Audience = JwtTokenHandlerOptions.Audience,
            Issuer = JwtTokenHandlerOptions.ValidIssuer,
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddSeconds(JwtTokenHandlerOptions.TokenExpirationInSeconds),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenHandlerOptions.SigningSecurityKey)), SecurityAlgorithms.HmacSha256Signature)
        };
    }

    private TokenValidationParameters CreateTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = JwtTokenHandlerOptions.Audience,
            ValidateIssuer = true,
            ValidIssuer = JwtTokenHandlerOptions.ValidIssuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenHandlerOptions.SigningSecurityKey)),
            ValidateActor = true,
            ValidateTokenReplay = true,
            ValidateLifetime = true
        };
    }
}
