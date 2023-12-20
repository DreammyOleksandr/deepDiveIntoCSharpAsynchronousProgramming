namespace TaskWhenAll.Services;

public interface IService<T>
{
    Task<List<T>> GetParseLocalJSON(string path);
    Task<List<T>> GetParseRemoteJSON(string url);
}