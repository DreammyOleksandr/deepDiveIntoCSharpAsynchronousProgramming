using Newtonsoft.Json;

string jsonPathLocal = @"/Users/bondarenkooleksandr/LocalDatabase.json";
var ParsedJSON = await GetParseLocalJSON(jsonPathLocal);

Console.WriteLine(ParsedJSON.ToString());

async Task<object> GetParseLocalJSON(string _jsonPath)
{
    string jsonContent = await File.ReadAllTextAsync(_jsonPath);
    var records = JsonConvert.DeserializeObject(jsonContent);
    
    return records;
}

