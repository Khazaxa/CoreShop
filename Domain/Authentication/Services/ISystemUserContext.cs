namespace Domain.Authentication.Services;

public interface ISystemUserContext : IUserContext
{
    Task InitializeAsync();
}
