namespace FCGames.Domain.Enums;

[Flags]
public enum AccessLevel
{
    Admin = 1,
    User = 2,
    Guest = 4
}