using System.Security.Cryptography;

namespace VictorKrogh.NET.Authentication.Services;

internal class PasswordHasherService : IPasswordHasherService
{
    private readonly RandomNumberGenerator _randomNumberGenerator;

    public PasswordHasherService(IPasswordHasherOptions options)
    {
        PasswordHasherOptions = options;
        _randomNumberGenerator = RandomNumberGenerator.Create();
    }

    protected IPasswordHasherOptions PasswordHasherOptions { get; }

    public byte[] GenerateSalt()
    {
        var salt = new byte[PasswordHasherOptions.SaltSize];

        _randomNumberGenerator.GetNonZeroBytes(salt);

        return salt;
    }

    public (byte[] HashedPassword, string HashAlgorithmName) HashPassword(byte[] salt, string password) => HashPassword(salt, password, PasswordHasherOptions.HashAlgorithmName!);

    public (byte[] HashedPassword, string HashAlgorithmName) HashPassword(byte[] salt, string password, string hashAlgoritmName)
    {
        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, PasswordHasherOptions.Iterations, new HashAlgorithmName(hashAlgoritmName));

        return (rfc2898DeriveBytes.GetBytes(PasswordHasherOptions.KeyLength), hashAlgoritmName);
    }

    public bool VerifyPassword(byte[] salt, byte[] hashedPassword, string plainTextPassword, string hashAlgoritmName)
    {
        var (password, _) = HashPassword(salt, plainTextPassword, hashAlgoritmName);

        return password.SequenceEqual(hashedPassword);
    }
}