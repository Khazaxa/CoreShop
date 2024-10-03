using Domain.Users.Enums;

namespace Domain.Authentication.Services;

public interface IUserContext
{
    int UserId { get; }
    string UserEmail { get; }
    UserRole? Role { get; }

    bool IsAdmin { get; }
    bool IsAuthenticated { get; }
}