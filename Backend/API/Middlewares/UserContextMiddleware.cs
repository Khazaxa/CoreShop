using API.Services;
using Domain.Authentication.Services;

namespace API.Middlewares;

public class UserContextMiddleware
{
    private readonly RequestDelegate _next;
    public UserContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IUserContextProvider _userContextProvider,
        HttpUserContext _httpUserContext)
    {
        _userContextProvider.Set(_httpUserContext);
        await _next(context);
    }
}
