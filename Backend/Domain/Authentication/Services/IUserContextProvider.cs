namespace Domain.Authentication.Services;

public interface IUserContextProvider
{
    bool HasValue { get; }

    IUserContext Get();
    void Set(IUserContext context); 
}