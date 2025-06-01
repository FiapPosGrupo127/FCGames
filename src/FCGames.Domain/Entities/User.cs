using FCGames.Domain.Enums;

namespace FCGames.Domain.Entities;

public class User : BaseEntity
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public AccessLevel AccessLevel { get; set; }

    public User() : base() { }
}