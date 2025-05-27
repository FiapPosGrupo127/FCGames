
namespace FCGames.API.Logs;

public class CustomLogger(string loggerName, CustomLoggerProviderConfiguration loggerConfig) : ILogger
{
    private readonly string loggerName = loggerName;
    private readonly CustomLoggerProviderConfiguration loggerConfig = loggerConfig;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string message = $"Log de execução: {logLevel} - {eventId.Id} - {formatter(state, exception)} - Executado em: {DateTime.Now}";

        Console.WriteLine(message);
    }
}