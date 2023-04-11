using System.Security.Cryptography;

namespace VictorKrogh.NET.Authentication.Handlers;

public interface IJwtRefreshTokenHandler
{
    string CreateToken();

    DateTime GetExpirationDate();
}

internal sealed class JwtRefreshTokenHandler : IJwtRefreshTokenHandler
{
    public JwtRefreshTokenHandler(IJwtRefreshTokenHandlerOptions jwtRefreshTokenHandlerOptions)
    {
        JwtRefreshTokenHandlerOptions = jwtRefreshTokenHandlerOptions;
    }
    
    private IJwtRefreshTokenHandlerOptions JwtRefreshTokenHandlerOptions { get; }

    public string CreateToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(randomBytes);
    }

    public DateTime GetExpirationDate()
    {
        return DateTime.UtcNow.AddSeconds(JwtRefreshTokenHandlerOptions.RefreshTokenExpirationInSeconds);
    }
}
