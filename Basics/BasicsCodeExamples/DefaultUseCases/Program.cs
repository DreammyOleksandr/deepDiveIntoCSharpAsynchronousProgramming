FamilyMember Alex = new("Alex", "Son", "vacuum cleaning");
FamilyMember Lila = new("Lila", "Mom", "flowers watering");

Console.WriteLine($"Current threadId: {Thread.CurrentThread.ManagedThreadId}");

//Uncomment next lines if you want to see different behaviour

// Lila.HouseMaintaining();
// Alex.HouseMaintaining();

// await Lila.HouseMaintainingAsync();
// Alex.HouseMaintaining();

// Lila.HouseMaintainingAsync();
// Alex.HouseMaintainingAsync();

await Lila.HouseMaintainingAsync();
await Alex.HouseMaintainingAsync();

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

        Console.WriteLine($"Current threadId: {Thread.CurrentThread.ManagedThreadId}");

        for (int minutesAmount = 1; minutesAmount <= estimatedTimeInMinutes; minutesAmount++)
        {
            Thread.Sleep(50);
            Console.WriteLine($"{_name} is {_chosenHouseWork} for {minutesAmount} minutes");
        }

        Console.WriteLine($"{_name} has done {_chosenHouseWork}");
        Console.WriteLine($"Current threadId: {Thread.CurrentThread.ManagedThreadId}");

    }

    public async Task HouseMaintainingAsync() =>
        await Task.Run(() => HouseMaintaining());
}