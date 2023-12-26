# Awaitable | [Code](../DeepDiveCodeExamples/Awaitable/)

## Awaitable Methods

Are **Asynchronous** methods the execution of which can be waited if you need their result at the moment. You should use operator `await` to wait for execution of such methods, but we have

## Other ways of awaiting

- Result prop.
- Methods: .Wait(), .WaitAll(), .WaitAny().
- Method .GetResult() from Method .GetAwaiter().

_Notice that .GetAwaiter() is not recommended for use of programmer. .GetAwaiter() is a method of TaskAwaiter structure which is more preferred to be used by the C# compiler rather than developer._

## Code examples:

Let's imagine the situation that Alex from our previous examples got a new task and now he should display all the data he got from local Database. He is also asked to make both Synchronous and Asynchronous functions that will do it,
_Notice that major part of code with some changes is copied from .WhenAll() section to keep flow of our narrative:_

Now, our IService.cs will look like this:

```csharp
namespace Awaitable.Services;

public interface IService<T>
{
    Task<List<T>> GetParseLocalJSON(string path);
    Task<List<T>> GetParseRemoteJSON(string url);
    Task DisplayAsync(string path);
    void Display(string path);
}
```

Service.cs:

```csharp
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
```

_.Display() and .DisplayAsync() Methods are the ones that are used in nearly all further examples:_

Calling Code:

```csharp
using Awaitable;
using Awaitable.Services;

string jsonPathLocal = @"/Users/bondarenkooleksandr/LocalDatabase.json";

Service<Customer> service = new();

service.Display(jsonPathLocal);
```

Now lets take a look at the different awaiting methods:

### Instance #1: await Operator

```csharp
await service.DisplayAsync(jsonPathLocal);
service.Display(jsonPathLocal);
```

### Instance #2: .Wait() Method

_.Wait(): Waits for the Task to complete execution. (Pretty simple and understandable)._

```csharp
service.DisplayAsync(jsonPathLocal).Wait();
service.Display(jsonPathLocal);
```

### Instance #3: .GetAwaiter().GetResult() Method

_.GetAwaiter(): Gets an awaiter to wait this Task._ </br>
_.GetResult(): Ends the wait for the completion of the asynchronous Task._ </br>
_Reminder that it is better to not use .GetAwaiter().GetResult due to the fact that it is purposed to be used by the compiler rather than developer._

```csharp
await service.DisplayAsync(jsonPathLocal);
service.Display(jsonPathLocal);
```

Results of these instances:

```console
Id: 1; First name: Piper, Last Name: Durdy, Email: pdurdy0@nytimes.com
Id: 2; First name: Elva, Last Name: Stearns, Email: estearns1@japanpost.jp
Id: 3; First name: Reinald, Last Name: Evangelinos, Email: revangelinos2@economist.com
...
Id: 998; First name: Valerye, Last Name: Crossthwaite, Email: vcrossthwaiterp@nps.gov
Id: 999; First name: Kearney, Last Name: Sprowle, Email: ksprowlerq@who.int
Id: 1000; First name: Ty, Last Name: Mellonby, Email: tmellonbyrr@craigslist.org
Id: 1; First name: Piper, Last Name: Durdy, Email: pdurdy0@nytimes.com
Id: 2; First name: Elva, Last Name: Stearns, Email: estearns1@japanpost.jp
Id: 3; First name: Reinald, Last Name: Evangelinos, Email: revangelinos2@economist.com
...
Id: 998; First name: Valerye, Last Name: Crossthwaite, Email: vcrossthwaiterp@nps.gov
Id: 999; First name: Kearney, Last Name: Sprowle, Email: ksprowlerq@who.int
Id: 1000; First name: Ty, Last Name: Mellonby, Email: tmellonbyrr@craigslist.org

Process finished with exit code 0.
```

Here we can see that every instance of awaiting has executed in the exact same way: Asynchronous methods were completed and right after that synchronous .`Display()` method continued the execution of the program.

### Instance #4: Result property

_For using Result property we need to execute method which returns something, so lets call .GetParseLocalJSON and check if it was awaited (We know that Database must contain 1000 records)._

```csharp
var result = service.GetParseLocalJSON(jsonPathLocal).Result;

if (result.Count != 1000) Console.WriteLine("Result variable does not contain whole Database set. That means that execution was not awaited and main thread went through further part of code.");
else Console.WriteLine("Result contains whole Database set. Execution of GetParseLocalJSON was awaited.");

// await service.DisplayAsync(jsonPathLocal);
// service.Display(jsonPathLocal);
```

Result:

```console
Result contains whole Database set. Execution of GetParseLocalJSON was awaited.

Process finished with exit code 0.
```

Now you know all of the standard awaiting methods in C#. Let's move on and find out about internal await execution.

## Awaitable Execution

For better understanding of how does await work lets Display our Thread Ids:

Calling code:

```csharp
using Awaitable;
using Awaitable.Services;

string jsonPathLocal = @"/Users/bondarenkooleksandr/LocalDatabase.json";

Service<Customer> service = new();

Console.WriteLine($"Start of app in Thread {Thread.CurrentThread.ManagedThreadId}");

service.DisplayAsync(jsonPathLocal);
service.Display(jsonPathLocal);

Console.WriteLine($"Start of app in Thread {Thread.CurrentThread.ManagedThreadId}");
```

Service.cs:

```csharp
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
        Console.WriteLine($"Start of Sync Method in Thread {Thread.CurrentThread.ManagedThreadId}");
        List<T> records = GetParseLocalJSON(path).Result;
        foreach (var record in records) Console.WriteLine(record.ToString());
        Console.WriteLine($"End of Sync Method in Thread {Thread.CurrentThread.ManagedThreadId}");
    }

    public async Task DisplayAsync(string path)
    {
        Console.WriteLine($"Start of Async Method in Thread {Thread.CurrentThread.ManagedThreadId}");
        await Task.Run(() => Display(path));
        Console.WriteLine($"End of Async Method in Thread {Thread.CurrentThread.ManagedThreadId}");
    }
}
```

Let's take a look at the execution of this code:
_Notice, that results may vary, due to the internal execution of an app._

```console
Start of calling code in Thread 1 // - Start of the app.
Start of Async Method in Thread 1 // - Start of the Async method.
Start of Sync Method in Thread 1 // - Start of the Sync method.
Start of Sync Method in Thread 7 // - Start of the Sync method, which is called in Async method.
Id: 1; First name: Piper, Last Name: Durdy, Email: pdurdy0@nytimes.com
Id: 2; First name: Elva, Last Name: Stearns, Email: estearns1@japanpost.jp
Id: 1; First name: Piper, Last Name: Durdy, Email: pdurdy0@nytimes.com
...
Id: 999; First name: Kearney, Last Name: Sprowle, Email: ksprowlerq@who.int
Id: 1000; First name: Ty, Last Name: Mellonby, Email: tmellonbyrr@craigslist.org
End of Sync Method in Thread 1 // - End of the Sync method.
End of Sync Method in Thread 7 // - End of the Sync method, which is called by Async method.
End of calling code in Thread 1 // - End of the app
End of Async Method in Thread 7 // - End of the Async method.

Process finished with exit code 0.
```

To be more precise and explain what happened we made a UML graph:

<p>
    <img src="./Awaitable Images/Awaitable Graph.png" alt="Awaitable Graph">
</p>

And here is what happens inside of await operator:

<p>
    <img src="./Awaitable Images/Await Execution Graph.png" alt="Inside of await">
</p>

Here we can see that if our result has not been returned than we pass controls back to the calling thread, which will wait until we get our results back. Only after that the execution of the method will resume.

**Thanks for reading this section! I hope you find out something new about await operator in C# :D.**
