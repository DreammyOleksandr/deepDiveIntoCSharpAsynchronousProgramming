# Default use cases | [Code](../BasicsCodeExample/DefaultUseCases)

Imagine the situation where family decided to do some housework. In our instance we will have a son who decided to do some vacuum cleaning and mom who decided to water flowers. (Indeed, based on their choice the time they will spend on housework will differ).

Let me explain what happens in our `FamilyMember` class:

```csharp
class FamilyMember
{
    public FamilyMember(string name, string familyRole, string chosenHouseWork)
    {
        _name = name;
        _familyRole = familyRole;
        _chosenHouseWork = chosenHouseWork;
    }

    private string _familyRole { get; set; }
    private string _name { get; set; }
    private string _chosenHouseWork { get; set; }

    public void HouseMaintaining()
    {
        int estimatedTimeInMinutes = ((Func<int>)(() =>
        {
            Dictionary<string, int> TimeEstimation = new()
            {
                ["vacuum cleaning"] = 40,
                ["flowers watering"] = 5,
            };
            return TimeEstimation[_chosenHouseWork];
        }))();

        for (int minutesAmount = 1; minutesAmount <= estimatedTimeInMinutes; minutesAmount++)
        {
            Thread.Sleep(50);
            Console.WriteLine($"{_name} is {_chosenHouseWork} for {minutesAmount} minutes");
        }

        Console.WriteLine($"{_name} has done {_chosenHouseWork} \n");
    }

    public async Task HouseMaintainingAsync() =>
        await Task.Run(() => HouseMaintaining());
}
```

Basically here we have specific props that are filled in constructor (Nothing fancy). Functions are more interesting ones here. We have _Synchronous_ `HouseMaintaining` which just evaluates the time which will be taken for housework execution and after executes usual for loop. _Asynchronous_ `HouseMaintainingAsync` that uses sync func, but wrapped with `await` operator. Now, let's take a look at the calling code:

```csharp
FamilyMember Alex = new("Alex", "Son", "vacuum cleaning");
FamilyMember Lila = new("Lila", "Mom", "flowers watering");

Lila.HouseMaintainingAsync();
Alex.HouseMaintaining();
```

Nothing fancy here too. Just usual instantiating of our entities and calling their methods, **_but, look how we call it (Async method comes first and the Sync one comes second). It will be important_**. Now let's look at some scenarios to understand how async programming works.

## Scenario #1: Sync functions calling

Let's see how it would be done in regular synchronous programming. For this, we have to call Sync `.HouseMaintaining` functions in calling part of code:

```csharp
Lila.HouseMaintaining();
Alex.HouseMaintaining();
```

Let's execute this app:

```console
Lila is flowers watering for 1 minutes
Lila is flowers watering for 2 minutes
Lila is flowers watering for 3 minutes
Lila is flowers watering for 4 minutes
Lila is flowers watering for 5 minutes
Lila has done flowers watering

Alex is vacuum cleaning for 1 minutes
Alex is vacuum cleaning for 2 minutes
Alex is vacuum cleaning for 3 minutes
Alex is vacuum cleaning for 4 minutes
Alex is vacuum cleaning for 5 minutes
...
Alex is vacuum cleaning for 38 minutes
Alex is vacuum cleaning for 39 minutes
Alex is vacuum cleaning for 40 minutes
Alex has done vacuum cleaning


Process finished with exit code 0.

```

I hope you've expected the same result :D. Just synchronous execution of a program. Nothing too fancy yet

If we will refer to the real world scenario it will look like this: Lila started watering flowers and right after she finished Alex started vacuum cleaning:

<p>
    <img src="./Default Use Cases Images/SyncDiagram.png">
</p>

Now let's take a look at case when they want to start doing housework at the same time

## Scenario #2: Async and Sync Functions calling

Lila decided to water flowers at the same time as Alex started cleaning:

```csharp
Lila.HouseMaintainingAsync();
Alex.HouseMaintaining();
```

Result:

```console
Lila is flowers watering for 1 minutes
Alex is vacuum cleaning for 1 minutes
Alex is vacuum cleaning for 2 minutes
Lila is flowers watering for 2 minutes
Lila is flowers watering for 3 minutes
Alex is vacuum cleaning for 3 minutes
Lila is flowers watering for 4 minutes
Alex is vacuum cleaning for 4 minutes
Lila is flowers watering for 5 minutes
Lila has done flowers watering

Alex is vacuum cleaning for 5 minutes
Alex is vacuum cleaning for 6 minutes
Alex is vacuum cleaning for 7 minutes
...
Alex is vacuum cleaning for 38 minutes
Alex is vacuum cleaning for 39 minutes
Alex is vacuum cleaning for 40 minutes
Alex has done vacuum cleaning


Process finished with exit code 0.
```

Here we can see that Alex and Lila were doing their work at the same time, or _In Parallel_ (But, it is not parallel by full meaning of a word, because in parallel programming all functions have to execute asynchronously, but in our case one func is executed synchronously on Main Thread and only second func is executed Asynchronously. To execute right parallel programming in C#, we have [`Task.WhenAll()`]() Method)

<p>
    <img src="./Default Use Cases Images/ParallelDiagram.png">
</p>

But let's take a look at the situation where Lila says "Hey, son I want to start doing my part of a work when you start yours" and Alex says: "Hey, son I want to start doing my part of a work when you start yours", so we will call 2 async methods.

## Scenario #3: Async Functions calling

Let's take a look at the scenario when both family members want to start doing their work as other one starts, but there is no strong initiator:

```csharp
Lila.HouseMaintainingAsync();
Alex.HouseMaintainingAsync();
```

Results can be surprising for some of you:

```console
Lila is flowers watering for 1 minutes
Alex is vacuum cleaning for 1 minutes

Process finished with exit code 0.
```

So, what's happening here? We can remember that when we call async methods the secondary threads take responsibilities for execution of them and in our case happens something like this:

<p>
    <img src="./Default Use Cases Images/BothAsyncDiagram.png">
</p>

From here you can understand if the main thread ended work of application then other threads won't continue their work (**Important:** _It works like this in console applications, but in other frameworks like WPF, WinForms, ASP.NET and others the execution of asynchronous functions can differ._)

Also we can have results like this:

```console

Process finished with exit code 0.
```

Or this:

```console

Lila is flowers watering for 2 minutes
Alex is vacuum cleaning for 2 minutes
Lila is flowers watering for 3 minutes
Alex is vacuum cleaning for 3 minutes
Lila is flowers watering for 4 minutes
Alex is vacuum cleaning for 4 minutes

Process finished with exit code 0.
```

Or any other variations that won't execute this application properly. It differs because of internal processes of app which can make main threat execute slower, or faster.

## Scenario #4: Awaited Async Functions calling

Now let's take a look at the right variant of using async functions and await them! (Let's also take a look at the threads managing specific methods by outputting them into console.)

```csharp
await Lila.HouseMaintainingAsync();
await Alex.HouseMaintainingAsync();
```

Result:

```console
Current threadId: 1
Current threadId: 4
Lila is flowers watering for 1 minutes
Lila is flowers watering for 2 minutes
Lila is flowers watering for 3 minutes
Lila is flowers watering for 4 minutes
Lila is flowers watering for 5 minutes
Lila has done flowers watering
Current threadId: 4
Current threadId: 9
Alex is vacuum cleaning for 1 minutes
Alex is vacuum cleaning for 2 minutes
Alex is vacuum cleaning for 3 minutes
...
Alex is vacuum cleaning for 38 minutes
Alex is vacuum cleaning for 39 minutes
Alex is vacuum cleaning for 40 minutes
Alex has done vacuum cleaning
Current threadId: 9

Process finished with exit code 0.
```

In our case the main thread is like father who firstly said to his wife to water flowers and after she did her work he forwarded towards son and told him to do his work:

<p>
    <img src="./Default Use Cases Images/AwaitDiagram.png">
</p>

(Notice that Ids given for secondary threads are not constant and can differ based on that which thread was called in thread pool by the application).

Now we have built more scalable application by calling `async` functions with `await` operator. But if you're attentive reader you could notice that awaited methods executed in the same way as regular synchronous methods and then you can ask: was there any difference in the speed of execution of this app? And I will answer to you: No! In this case we just took care of managing our threads. If you wish to know how to make applications with asynchronous functions execute much faster read our [Task.WhenAll() guide]().
