async Task HouseMaintenance(string housework, int estimatedTime)
{
    await Task.Run(() =>
    {
        for (int secondsAmount = 1; secondsAmount < estimatedTime; secondsAmount++)
        {
            Console.WriteLine($"Doing {housework} for {secondsAmount} seconds");
        }
    });
}

//This program does nothing. It is presented only to show the syntax of async/await.