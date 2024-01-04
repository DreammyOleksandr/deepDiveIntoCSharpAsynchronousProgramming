# Continuations | [Code]()

- [Continuations | Code](#continuations--code)
  - [Theory and definitions](#theory-and-definitions)
  - [Code Example](#code-example)
  - [Conclusion](#conclusion)

## Theory and definitions

Continuations are mechanisms that allow developer write the continuation functions with result using of antecedent function when is is completed. Also to create continuations(chaining) more often you use _callback_ functions, but with this mechanism, which is executed through `Task.ContinueWith()` method we can easily track the status of the antecedent method, or cancel it.

## Code Example

For these app you need a **[Newtonsoft.Json](https://www.newtonsoft.com/json)** NuGet Package to be installed.

_Notice that major part of code with some changes is copied from .WhenAll() section to keep flow of our narrative:_

The situation is following: We have two databases and for each we need to

- Get the whole dataset from each Db;
- Filter the data by some regex of email;
- Output new dataset.

Let's explore how our functions will look using `ContinueWith()`:

```csharp
await service.GetParseLocalJSON(firstLocalDbPath)
    .ContinueWith(taskResult =>
    {
        return (taskResult.Status == TaskStatus.Faulted)
            ? throw new Exception("Something went wrong")
            : taskResult.Result.Where(x => x.Email.Contains("ol"));
    })
    .ContinueWith(taskResult =>
    {
        IEnumerable<Customer> customers = taskResult.Result;
        foreach (var customer in customers)
            Console.WriteLine(customer.ToString());
    });

await service.GetParseLocalJSON(secondLocalDbPath)
    .ContinueWith(taskResult =>
    {
        return (taskResult.Status == TaskStatus.Faulted)
            ? throw new Exception("Something went wrong")
            : taskResult.Result.Where(x => x.Email.Contains("ol"));
    })
    .ContinueWith(taskResult =>
    {
        IEnumerable<Customer> customers = taskResult.Result;
        foreach (var customer in customers)
            Console.WriteLine(customer.ToString());
    });
```

Here we can notice that there are some validations for completion of the task, so let's fail this task to see if it actually works. For this we will just change the path of databases to the wrong one:

```csharp
string firstLocalDbPath = @"/Users/bondarenkooleksandr/LocalDatabase.jso"; // <-- Must be .json
string secondLocalDbPath = @"/Users/bondarenkooleksandr/SecondDataBase.json";
```

Result:

```console
Unhandled exception. System.AggregateException: One or more errors occurred. (Something went wrong)
 ---> System.Exception: Something went wrong
...
```

Nice! The continuation validated the state of the antecedent function and threw an error. Now let's see the proper result of working code:

```console
Id: 7; First name: Imogen, Last Name: Waterson, Email: iwaterson6@purevolume.com
Id: 18; First name: Gabi, Last Name: Oldridge, Email: goldridgeh@examiner.com
Id: 48; First name: Larine, Last Name: O'Nolan, Email: lonolan1b@amazon.de
...
Id: 287; First name: Jennie, Last Name: Gollop, Email: jgollop7y@umich.edu
Id: 290; First name: Ruperta, Last Name: McElane, Email: rmcelane81@purevolume.com
Id: 295; First name: Elnora, Last Name: Colbert, Email: ecolbert86@yale.edu

Id: 18; First name: Eugenie, Last Name: Hollebon, Email: ehollebonh@elpais.com
Id: 37; First name: Cosimo, Last Name: Driscoll, Email: cdriscoll10@vistaprint.com
Id: 40; First name: Arabel, Last Name: Bartolijn, Email: abartolijn13@epa.gov
...
Id: 232; First name: Portia, Last Name: Kollas, Email: pkollas6f@de.vu
Id: 254; First name: Noni, Last Name: Bothwell, Email: nbothwell71@uol.com.br
Id: 288; First name: Lilli, Last Name: Leopold, Email: lleopold7z@wp.com

Process finished with exit code 0.
```

Here we can see that every email got the needed "ol" part and everything worked perfectly.

## Conclusion

In modern C# versions, it's often more convenient to use the `async/await` pattern for managing asynchronous operations, but `Task.ContinueWith()` can still be useful in certain scenarios, especially when dealing with non-async code or more complex task flow control.
