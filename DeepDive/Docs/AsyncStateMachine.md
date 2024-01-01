# Async State Machine | [Code](../DeepDiveCodeExamples/AsyncStateMachine/)

## Theory and definitions

This particular part presents you one of the main C# Asynchronous programming concepts: **Asynchronous State Machine**. This exact code structure is hidden behind the `async/await` keywords and all of the _magic_ is done by it.

C# _**Asynchronous State Machine**_ is a code abstraction provided by C# that manages the asynchronous operations in your code and is created in _Low-level C#_ when we use `async/await` keywords.

In Asynchronous _State_ Machine we can notice the word _State_. Let's see what _States_ of Asynchronous State Machine exist:

```C#
enum State
{
    Completed = -2,
    Created = -1,
    Awaiting = 0,
}
```

## Code Example

For these app you need a **[Newtonsoft.Json](https://www.newtonsoft.com/json)** NuGet Package to be installed.

Keep moving forward and let us navigate you through our code example (which was used in [Task.WhenAll()](../../Basics/BasicsCodeExamples/TaskWhenAll/TaskWhenAll/) section) by showing executable functions and calling code:

The [IService](../DeepDiveCodeExamples/AsyncStateMachine/AsyncStateMachine/Services/IService.cs) interface which is implemented by Service class:

```C#
namespace AsyncStateMachine.Services;

public interface IService
{
    Task<object> GetParseLocalJSONAsync();
    object GetParseLocalJSON();
}
```

[Service](../DeepDiveCodeExamples/AsyncStateMachine/AsyncStateMachine/Services/Service.cs) Class with executable functions:

```C#
using Newtonsoft.Json;

namespace AsyncStateMachine.Services;

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
```

We can see that `Service` defines the functions of implemented `IService` interface. `GetParseLocalJSON()` and `GetParseLocalJSONAsync()` return us parsed JSON objects synchronously and and asynchronously respectively.

Now let's take a look at our calling code in [Program](../DeepDiveCodeExamples/AsyncStateMachine/AsyncStateMachine/Program.cs) class:

```C#
using AsyncStateMachine.Services;

string jsonPathLocal = @"/Users/bondarenkooleksandr/LocalDatabase.json";
IService service = new Service(jsonPathLocal);

Console.WriteLine($"{await service.GetParseLocalJSONAsync()}\n{service.GetParseLocalJSON()}");
```

Here we define our JSON local path then instantiate our `Service` with this path and after that we output the execution of `GetParseLocalJSON()` and `GetParseLocalJSONAsync()` methods. Now we can execute our code to see if it actually works:

```console
[
  {
    "id": 1,
    "first_name": "Piper",
    "last_name": "Durdy",
    "email": "pdurdy0@nytimes.com"
  },
  {
    "id": 2,
    "first_name": "Elva",
    "last_name": "Stearns",
    "email": "estearns1@japanpost.jp"
  },

...

  {
    "id": 299,
    "first_name": "Mozes",
    "last_name": "Oen",
    "email": "moen8a@marriott.com"
  },
  {
    "id": 300,
    "first_name": "Kessiah",
    "last_name": "Mustill",
    "email": "kmustill8b@vimeo.com"
  }
]
[
  {
    "id": 1,
    "first_name": "Piper",
    "last_name": "Durdy",
    "email": "pdurdy0@nytimes.com"
  },
  {
    "id": 2,
    "first_name": "Elva",
    "last_name": "Stearns",
    "email": "estearns1@japanpost.jp"
  },

...

  {
    "id": 299,
    "first_name": "Mozes",
    "last_name": "Oen",
    "email": "moen8a@marriott.com"
  },
  {
    "id": 300,
    "first_name": "Kessiah",
    "last_name": "Mustill",
    "email": "kmustill8b@vimeo.com"
  }
]
```

Yep, our functions have outputted in the same way. App works perfectly.

## Decompiled Code
