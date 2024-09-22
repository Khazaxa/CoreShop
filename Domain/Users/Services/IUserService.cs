using Domain.Users.Enums;

namespace Domain.Users.Services;

public interface IUserService
{
    Task ValidateIfEmailExistsAsync(string email, CancellationToken cancellationToken);

    Task CreateInitialUserAsync(string email, 
        string password, 
        string name, 
        string surname, 
        string areaCode, 
        string phone, 
        CancellationToken cancellationToken
    );

}