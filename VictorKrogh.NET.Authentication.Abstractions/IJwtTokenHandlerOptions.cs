namespace VictorKrogh.NET.Authentication;

public interface IJwtTokenHandlerOptions
{
    string Audience { get; }
    string ValidIssuer { get; }
    string SigningSecurityKey { get; }
    string EncryptingSecurityKey { get; }

    int TokenExpirationInSeconds { get; }
}
