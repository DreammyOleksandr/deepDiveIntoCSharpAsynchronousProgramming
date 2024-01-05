using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Continuations.Services;

public class Service<T> : IService<T> where T : class
{
    public async Task<List<T>> GetParseLocalJSON(string _jsonPath) =>
        JsonConvert.DeserializeObject<List<T>>(await File.ReadAllTextAsync(_jsonPath));

    public async Task<List<T>> GetParseRemoteJSON(string url)
    {
        using HttpClient client = new HttpClient();
        return JsonConvert.DeserializeObject<List<T>>(await client.GetStringAsync(url));
    }
}