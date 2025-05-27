namespace FCGames.Domain.Configuration;

public class TokenConfiguration
{
    public required string Key { get; set; } 
    public int ExpirationTimeHour { get; set; }
    public int IncreaseExpirationTimeMinutes { get; set; }

}