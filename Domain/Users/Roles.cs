using Domain.Users.Enums;

namespace Domain.Users;

public class Roles 
{
    public const string Admin = nameof(UserRole.Admin);
    public const string Employee = nameof(UserRole.Employee);
    public const string Client = nameof(UserRole.Client);
}