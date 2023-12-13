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
        Dictionary<string, int> EstimatedTimeInMinutes = new()
        {
            ["vacuum cleaning"] = 40,
            ["flowers watering"] = 5,
        };

        int estimatedTime = EstimatedTimeInMinutes[_chosenHouseWork];

        for (int minutesAmount = 1; minutesAmount <= estimatedTime; minutesAmount++)
        {
            Thread.Sleep(50);
            Console.WriteLine($"{_name} is {_chosenHouseWork} for {minutesAmount} minutes");
        }
        Console.WriteLine($"{_name} has done {_chosenHouseWork}");
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

## Scenario #1: Sync function calling

Let's see how it would be done in regular synchronous programming. For this, we have to