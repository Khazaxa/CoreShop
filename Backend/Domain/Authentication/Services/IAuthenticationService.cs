using Domain.Users.Enums;

namespace Domain.Authentication.Services;

public interface IAuthenticationService
{
    string GenerateToken(string email, UserRole role, int userId);
    byte[] ComputePasswordHash(string password, byte[] salt);
}