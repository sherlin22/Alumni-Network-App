using AlumniNetwork.Application.Interfaces;

namespace AlumniNetwork.Infrastructure.JWTServices;

public class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
}
