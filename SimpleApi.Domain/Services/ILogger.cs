namespace SimpleApi.Domain.Services;

public interface ILogger
{
    //Task<bool> Log(string info);
    Task<bool> Log(string info, string stream = null);
    Task Error(string info, Exception stacktrace, string stream = null);
}