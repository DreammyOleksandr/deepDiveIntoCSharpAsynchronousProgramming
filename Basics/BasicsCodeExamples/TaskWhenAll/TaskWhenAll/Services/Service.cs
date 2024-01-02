using Newtonsoft.Json;

namespace TaskWhenAll.Services;

public class Service<T> : IService<T> where T : class
{
    public async Task<List<T>> GetParseLocalJSON(string _jsonPath)
    {
        await Task.Delay(1000);
        return JsonConvert.DeserializeObject<List<T>>(await File.ReadAllTextAsync(_jsonPath));
    }

    public async Task<List<T>> GetParseRemoteJSON(string url)
    {
        await Task.Delay(1000);
        using HttpClient client = new HttpClient();
        
        return JsonConvert.DeserializeObject<List<T>>(await client.GetStringAsync(url));
    }
}