using Awaitable;
using Awaitable.Services;

string jsonPathLocal = @"/Users/bondarenkooleksandr/LocalDatabase.json";

Service<Customer> service = new();

//Instance #1
await service.DisplayAsync(jsonPathLocal);

//Instance #2
// service.DisplayAsync(jsonPathLocal).Wait();

//Instance #3
service.DisplayAsync(jsonPathLocal).GetAwaiter().GetResult();

// Instance #4
// var result = service.GetParseLocalJSON(jsonPathLocal).Result;
//
// if (result.Count != 1000) Console.WriteLine("Result variable does not contain whole Database set. That means that execution was not awaited and main thread went through further part of code.");
// else Console.WriteLine("Result contains whole Database set. Execution of GetParseLocalJSON was awaited.");
//
// await service.DisplayAsync(jsonPathLocal);

service.Display(jsonPathLocal);