using System.Diagnostics;
using TaskWhenAll;
using TaskWhenAll.Services;

string jsonPathLocal = @"/Users/bondarenkooleksandr/LocalDatabase.json";
string jsonUrlRemote = @"https://raw.githubusercontent.com/DreammyOleksandr/DeepDiveIntoCSharpAsynchroniusProgramming/main/Basics/Source/RemoteDatabase.json";

Service<Customer> service = new();

Stopwatch sw = new();

sw.Start();

// List<Customer> LocalCustomers = await service.GetParseLocalJSON(jsonPathLocal);
// List<Customer> RemoteCustomers = await service.GetParseRemoteJSON(jsonUrlRemote);

var localCustomersTask = service.GetParseLocalJSON(jsonPathLocal);
var RemoteCustomersTask = service.GetParseRemoteJSON(jsonUrlRemote);

await Task.WhenAll(localCustomersTask, RemoteCustomersTask);

List<Customer> LocalCustomers = localCustomersTask.Result;
List<Customer> RemoteCustomers = RemoteCustomersTask.Result;

sw.Stop();

Console.WriteLine(sw.Elapsed.TotalSeconds);
