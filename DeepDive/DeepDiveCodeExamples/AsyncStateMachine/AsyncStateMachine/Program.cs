using Newtonsoft.Json;

string jsonPathLocal = @"/Users/bondarenkooleksandr/LocalDatabase.json";

Console.WriteLine(await GetParseLocalJSONAsync(jsonPathLocal));
Console.WriteLine(GetParseLocalJSON(jsonPathLocal));

object GetParseLocalJSON(string _jsonPath) => JsonConvert.DeserializeObject(File.ReadAllText(_jsonPath));
async Task<object> GetParseLocalJSONAsync(string _jsonPath) => JsonConvert.DeserializeObject(await File.ReadAllTextAsync(_jsonPath));