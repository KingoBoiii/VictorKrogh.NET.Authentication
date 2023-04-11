namespace VictorKrogh.NET.Authentication;

public interface IJwtRefreshTokenHandlerOptions
{
    int RefreshTokenExpirationInSeconds { get; }
}
