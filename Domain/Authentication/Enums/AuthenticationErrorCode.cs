namespace Domain.Authentication.Enums;

public enum AuthenticationErrorCode
{
    UserOrPasswordIncorrect = 1,
    JwtExpireMinutesNotConfigured = 2
}