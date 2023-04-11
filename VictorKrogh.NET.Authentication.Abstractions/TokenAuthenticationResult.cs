namespace VictorKrogh.NET.Authentication;

public class TokenAuthenticationResult
{
    public string? Token { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public string? RefreshToken { get; set; }
}
