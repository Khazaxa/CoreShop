namespace Domain.Users.Services;

public interface IUserService
{
    Task ValidateIfEmailExistsAsync(string email, CancellationToken cancellationToken);
}