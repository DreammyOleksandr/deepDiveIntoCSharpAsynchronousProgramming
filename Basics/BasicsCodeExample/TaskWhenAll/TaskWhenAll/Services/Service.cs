using Newtonsoft.Json;

namespace TaskWhenAll.Services;

public class Service<T> : IService<T> where T : class
{
    public async Task<List<T>> GetParseLocalJSON(string _jsonPath)
    {
        await Task.Delay(1000);
        
        string jsonContent = await File.ReadAllTextAsync(_jsonPath);
        List<T> records = JsonConvert.DeserializeObject<List<T>>(jsonContent);
        
        return records;
    }

    public async Task<List<T>> GetParseRemoteJSON(string url)
    {
        await Task.Delay(1000);

        using HttpClient client = new HttpClient();
        string jsonContent = await client.GetStringAsync(url);
        List<T> records = JsonConvert.DeserializeObject<List<T>>(jsonContent);

        return records;
    }
}