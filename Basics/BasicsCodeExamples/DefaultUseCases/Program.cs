using DefaultUseCases;

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
