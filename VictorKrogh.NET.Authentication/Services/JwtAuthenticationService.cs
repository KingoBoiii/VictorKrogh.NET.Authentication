using VictorKrogh.NET.Authentication.Handlers;

namespace VictorKrogh.NET.Authentication;

public interface IJwtAuthenticationService
{
    Task<TokenAuthenticationResult> AuthenticateAsync(IJwtUser jwtUser);
}

internal sealed class JwtAuthenticationService : IJwtAuthenticationService
{
	public JwtAuthenticationService(IJwtTokenHandler jwtTokenHandler, IJwtRefreshTokenHandler jwtRefreshTokenHandler)
	{
        JwtTokenHandler = jwtTokenHandler;
        JwtRefreshTokenHandler = jwtRefreshTokenHandler;
    }

    private IJwtTokenHandler JwtTokenHandler { get; }
    private IJwtRefreshTokenHandler JwtRefreshTokenHandler { get; }

    public async Task<TokenAuthenticationResult> AuthenticateAsync(IJwtUser jwtUser)
    {
        var token = await JwtTokenHandler.CreateJwtTokenAsync(jwtUser);

        var refreshToken = JwtRefreshTokenHandler.CreateToken();

        return new TokenAuthenticationResult
        {
            Token = token,
            RefreshToken = refreshToken
        };
    }
}
