using Newtonsoft.Json;

namespace AsyncStateMachine_Decompiled.Services;

public class Service : IService
{
    private readonly string _jsonPath;
    public Service(string jsonPath)
    {
        _jsonPath = jsonPath;
    }
    
    public object GetParseLocalJSON() => 
        JsonConvert.DeserializeObject(File.ReadAllText(_jsonPath));

    public async Task<object> GetParseLocalJSONAsync() => 
        JsonConvert.DeserializeObject(await File.ReadAllTextAsync(_jsonPath));
}