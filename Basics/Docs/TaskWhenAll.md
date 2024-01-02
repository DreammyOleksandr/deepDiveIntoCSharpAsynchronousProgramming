# Task.WhenAll() | [Code](../BasicsCodeExample/TaskWhenAll)

For these app you need a **[Newtonsoft.Json](https://www.newtonsoft.com/json)** NuGet Package to be installed.

Hello, dear reader! I hope you have got acquainted with our [basic](../README.md) and [default use cases](./DefaultUseCases.md) sections. If so, let's continue our trip and let's take a look how to execute our application _In Parallel_ and _Faster_ with using Task.WhenAll().

Our example will be more realistic this time.

Let's take a look at it: Alex has found a job in IT and his first Task was to write an implementation of Get methods, that get customers' data from two separate databases (One is local and the second is remote) and parse it to C# objects.

For database content filling I use **[Mockaroo](https://www.mockaroo.com/)**.

Our records will have Id, first name, last name and Email, so their wrapper class will look like this:

```csharp
namespace TaskWhenAll;

public class Customer
{
    public int Id { get; set; }
    public string first_Name { get; set; }
    public string last_Name { get; set; }
    public string Email { get; set; }

    public override string ToString() => $"Id: {Id}; First name: {first_Name}, Last Name: {last_Name}, Email: {Email}";
}
```

_Notice that I don't use ToString() method in this project. I have overridden it for you in case you want to output the results into console._

Alex did his tasks and created needed service and abstraction for it:

```csharp
namespace TaskWhenAll.Services;

public interface IService<T>
{
    Task<List<T>> GetParseLocalJSON(string path);
    Task<List<T>> GetParseRemoteJSON(string url);
}
```

```csharp
using Newtonsoft.Json;

namespace TaskWhenAll.Services;

public class Service<T> : IService<T> where T : class
{
    public async Task<List<T>> GetParseLocalJSON(string _jsonPath)
    {
        await Task.Delay(1000);
        return JsonConvert.DeserializeObject<List<T>>(await File.ReadAllTextAsync(_jsonPath));
    }

    public async Task<List<T>> GetParseRemoteJSON(string url)
    {
        await Task.Delay(1000);
        using HttpClient client = new HttpClient();

        return JsonConvert.DeserializeObject<List<T>>(await client.GetStringAsync(url));
    }
}
```

Here we have a `IService` interface abstraction with `GetParseLocalJSON()` and `GetParseRemoteJSON()` for our `Service` class or other possible future implementors. In `Service` class we have an implementation of Interface methods. Everything is pretty simple, you can see that these functions Get and Parse **JSON** files to C# objects either from local or remote databases.

_Notice that I have a delay to simulate some latency to make examples more demonstrative._

Now, let's take a look at our calling code:

## Scenario #1: Default awaiting

In the following instance we await both methods without using Task.WhenAll(). Let's see the calling code and the execution:

```csharp
using System.Diagnostics;
using TaskWhenAll;
using TaskWhenAll.Services;

string jsonPathLocal = @"/Users/bondarenkooleksandr/LocalDatabase.json";
string jsonUrlRemote = @"https://raw.githubusercontent.com/DreammyOleksandr/DeepDiveIntoCSharpAsynchroniusProgramming/main/Basics/Source/RemoteDatabase.json";

Service<Customer> service = new();

Stopwatch sw = new();

sw.Start();

List<Customer> LocalCustomers = await service.GetParseLocalJSON(jsonPathLocal);
List<Customer> RemoteCustomers = await service.GetParseRemoteJSON(jsonUrlRemote);

sw.Stop();

Console.WriteLine(sw.Elapsed.TotalSeconds);
```

Results after 3 starts of the app:

```console
2,6275395

Process finished with exit code 0.
```

```console
2,44206

Process finished with exit code 0.
```

```console
2,4224

Process finished with exit code 0.
```

Obviously, the time execution of this app is ~2,5 seconds.

<p>
    <img src="./Task WhenAll Images/NoWhenAll.png">
</p>

## Scenario #1: Task.WhenAll() using

In the following instance we use Task.WhenAll(). Here we store our async tasks to `Task` variables, then get their results in `Task.WhenAll` and pass these results to Lists. Let's see the calling code and the execution:

```csharp
using System.Diagnostics;
using TaskWhenAll;
using TaskWhenAll.Services;

string jsonPathLocal = @"/Users/bondarenkooleksandr/LocalDatabase.json";
string jsonUrlRemote = @"https://raw.githubusercontent.com/DreammyOleksandr/DeepDiveIntoCSharpAsynchroniusProgramming/main/Basics/Source/RemoteDatabase.json";

Service<Customer> service = new();

Stopwatch sw = new();

sw.Start();

var localCustomersTask = service.GetParseLocalJSON(jsonPathLocal);
var RemoteCustomersTask = service.GetParseRemoteJSON(jsonUrlRemote);

await Task.WhenAll(localCustomersTask, RemoteCustomersTask);

List<Customer> LocalCustomers = localCustomersTask.Result;
List<Customer> RemoteCustomers = RemoteCustomersTask.Result;

sw.Stop();

Console.WriteLine(sw.Elapsed.TotalSeconds);

```

Results after 3 starts of the app:

```console
1,4981578

Process finished with exit code 0.
```

```console
1,3219647

Process finished with exit code 0.
```

```console
1,326652

Process finished with exit code 0.
```

I suppose, it is also obvious that the execution of this version of app is ~1,5 seconds. So, what is happening here:

<p>
    <img src="./Task WhenAll Images/WhenAll.png">
</p>

`Task.WhenAll()` is like a peculiar wrapper for methods that tells them to start their execution _In Parallel_ and when methods are executed it returns their context with which user can operate after. In our case we just got values of executed functions with `.Value` property. We can notice that this version of app doesn't have such problem like in [Scenario #3 of Default use cases](./DefaultUseCases.md).

Summarizing, `Task.WhenAll()` is a valuable tool for managing multiple asynchronous tasks in C#. By understanding how to use `Task.WhenAll()` effectively, you can write efficient and responsive asynchronous code in your C# applications.
