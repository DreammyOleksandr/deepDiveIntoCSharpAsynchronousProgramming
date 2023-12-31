using Newtonsoft.Json;

string jsonPathLocal = @"/Users/bondarenkooleksandr/LocalDatabase.json";
object ParsedJSON = await GetParseLocalJSON(jsonPathLocal);

Console.WriteLine(ParsedJSON);

async Task<object> GetParseLocalJSON(string _jsonPath)
{
    string jsonContent = await File.ReadAllTextAsync(_jsonPath);
    object records = JsonConvert.DeserializeObject<List<object>>(jsonContent);
    
    return records;
}

