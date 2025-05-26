using FCGames.Domain.Enums;

namespace FCGames.Domain.Entities;

public class User : BaseEntity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public AccessLevel AccessLevel { get; set; }

    public User() : base() { }
}