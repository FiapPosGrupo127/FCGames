using System.Collections.Concurrent;

namespace FCGames.API.Logs;

public class CustomLoggerProvider(CustomLoggerProviderConfiguration _loggerConfig) : ILoggerProvider
{
    private readonly CustomLoggerProviderConfiguration loggerConfig = _loggerConfig;
    private readonly ConcurrentDictionary<string, CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger>();

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, loggerConfig));
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}