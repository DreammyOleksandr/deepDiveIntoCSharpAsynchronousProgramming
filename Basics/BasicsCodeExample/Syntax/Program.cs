async Task HouseMaintenance(string housework, int estimatedTime)
{
    await Task.Run(() =>
    {
        for (int minutesAmount = 1; minutesAmount <= estimatedTime; minutesAmount++)
        {
            Console.WriteLine($"Doing {housework} for {minutesAmount} minutes");
        }
    });
}

//This program does nothing. It is presented only to show the syntax of async/await.