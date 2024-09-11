using System.Security.Claims;
using Domain.Users.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.Authentication.Services;

public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    public ClaimsPrincipal GetCurrentUser()
        => httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();

    public bool IsAdmin()
    {
        var user = GetCurrentUser();
        return user.IsInRole(UserRole.Admin.ToString());
    }
}