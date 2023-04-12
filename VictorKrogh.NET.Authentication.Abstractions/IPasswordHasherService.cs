namespace VictorKrogh.NET.Authentication;

public interface IPasswordHasherService
{
    byte[] GenerateSalt();
    (byte[] HashedPassword, string HashAlgorithmName) HashPassword(byte[] salt, string password);
    (byte[] HashedPassword, string HashAlgorithmName) HashPassword(byte[] salt, string password, string hashAlgorithmName);
    bool VerifyPassword(byte[] salt, byte[] hashedPassword, string plainTextPassword, string hashAlgorithmName);
}