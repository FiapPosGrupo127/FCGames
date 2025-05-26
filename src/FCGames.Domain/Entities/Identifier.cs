using FCGames.Domain.Entities.Interfaces;

namespace FCGames.Domain.Entities;

public abstract class Identifier : IIdentifier
{
    public Guid Id { get; set; }
}