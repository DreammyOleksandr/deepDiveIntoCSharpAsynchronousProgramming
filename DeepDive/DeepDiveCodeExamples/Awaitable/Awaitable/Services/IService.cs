namespace Awaitable.Services;

public interface IService<T>
{
    Task<List<T>> GetParseLocalJSON(string path);
    Task<List<T>> GetParseRemoteJSON(string url);
    Task DisplayAsync(string path);
    void Display(string path);
}