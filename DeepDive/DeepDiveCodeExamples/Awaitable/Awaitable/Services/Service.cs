using Newtonsoft.Json;

namespace Awaitable.Services;

public class Service<T> : IService<T> where T : class
{
    public async Task<List<T>> GetParseLocalJSON(string path)
    {
        string jsonContent = await File.ReadAllTextAsync(path);
        List<T> records = JsonConvert.DeserializeObject<List<T>>(jsonContent);

        return records;
    }

    public async Task<List<T>> GetParseRemoteJSON(string url)
    {
        using HttpClient client = new HttpClient();
        string jsonContent = await client.GetStringAsync(url);
        List<T> records = JsonConvert.DeserializeObject<List<T>>(jsonContent);

        return records;
    }

    public void Display(string path)
    {
        List<T> records = GetParseLocalJSON(path).Result;
        foreach (var record in records) Console.WriteLine(record.ToString());
    }
    
    public async Task DisplayAsync(string path) => await Task.Run(() => Display(path));
}