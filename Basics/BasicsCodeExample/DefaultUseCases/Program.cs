FamilyMember Alex = new("Alex", "Son", "vacuum cleaning");
FamilyMember Lila = new("Lila", "Mom", "flowers watering");

Lila.HouseMaintaining();
Alex.HouseMaintaining();

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