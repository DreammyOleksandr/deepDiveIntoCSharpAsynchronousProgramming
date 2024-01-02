using AsyncStateMachine_Decompiled.Services;

string jsonPathLocal = @"/Users/bondarenkooleksandr/LocalDatabase.json";
IService service = new Service(jsonPathLocal);

Console.WriteLine($"{await service.GetParseLocalJSONAsync()}\n{service.GetParseLocalJSON()}");