namespace Domain.Authentication.Services;

internal class UserContextProvider : IUserContextProvider
{
    private IUserContext? _context;

    public bool HasValue => _context?.IsAuthenticated  == true;

    public IUserContext Get() => _context ?? throw new Exception("User context not initialized");

    public void Set(IUserContext context) => _context = context;
}
