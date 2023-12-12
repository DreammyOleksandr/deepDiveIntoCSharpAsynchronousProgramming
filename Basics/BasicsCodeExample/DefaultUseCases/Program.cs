FamilyMember Alex = new("Alex", "Son", "vacuum cleaning");
FamilyMember Lila = new("Lila", "Mom", "flowers watering");

Dictionary<string, int> TimeEstimated = new()
{
    ["vacuum cleaning"] = 40,
    ["flowers watering"] = 5,
};

Lila.HouseMaintainingAsync(TimeEstimated[Lila.ChosenHouseWork.ToLower()]);
Alex.HouseMaintaining(TimeEstimated[Alex.ChosenHouseWork.ToLower()]);

class FamilyMember
{
    public FamilyMember(string name, string familyRole, string chosenHouseWork)
    {
        _name = name;
        _familyRole = familyRole;
        ChosenHouseWork = chosenHouseWork;
    }

    private string _familyRole { get; set; }
    private string _name { get; set; }
    public string ChosenHouseWork { get; set; }
    
    public void HouseMaintaining(int estimatedTime)
    {
        for (int minutesAmount = 1; minutesAmount <= estimatedTime; minutesAmount++)
        {
            Thread.Sleep(50);
            Console.WriteLine($"{_name} is {ChosenHouseWork} for {minutesAmount} minutes");
        }
        Console.WriteLine($"{_name} has done {ChosenHouseWork}");
    }
    
    public async Task HouseMaintainingAsync(int estimatedTime) =>
        await Task.Run(() => HouseMaintaining(estimatedTime));
}