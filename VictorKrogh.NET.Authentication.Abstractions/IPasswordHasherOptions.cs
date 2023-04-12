namespace VictorKrogh.NET.Authentication;

public interface IPasswordHasherOptions
{
    int Iterations { get; }
    int SaltSize { get; }
    int KeyLength { get; }
    string HashAlgorithmName { get; }
}
