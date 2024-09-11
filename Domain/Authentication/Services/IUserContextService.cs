using System.Security.Claims;

namespace Domain.Authentication.Services;

public interface IUserContextService
{
    ClaimsPrincipal GetCurrentUser();
    bool IsAdmin();
}