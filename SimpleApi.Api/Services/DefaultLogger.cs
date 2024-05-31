using Serilog;
using ILogger = SimpleApi.Domain.Services.ILogger;

namespace SimpleApi.API.Services;

public class DefaultLogger : ILogger
{
    //private readonly Logger _logger;
    public DefaultLogger()
    {
    }

    public static void ConfigureLogger(string path)
    {
        Serilog.Log.Logger = new LoggerConfiguration()
                             .MinimumLevel.Debug()
                             .WriteTo.File(path, rollingInterval: RollingInterval.Day)
                             .CreateLogger();
    }

    public async Task<bool> Log(string info, string stream = null)
    {
        Serilog.Log.Information(info);
        return true;
    }

    public async Task Error(string info, Exception exception, string stream = null)
    {
        Serilog.Log.Error(info + "\r\n" + exception.Message + "\r\n" + exception.StackTrace);
    }
}