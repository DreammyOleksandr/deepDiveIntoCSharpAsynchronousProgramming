int freeHotplates = 4;
int averageHotplatesUse = 3;

// tell four students that it's time for lunch
for (int i = 1; i <= 4; i++)
{
    Thread student = new Thread(PrepareDish);
    student.Name = $"Student {i}";
    student.Start(averageHotplatesUse);
}

void PrepareDish(object? obj)
{
    if (obj is int neededHotplates)
    {
        // all students try to cook a dish
        string currentStudent = Thread.CurrentThread.Name!;
        int usedHotplates = GetFreeHotplates(neededHotplates);
        float dishReadiness = 0;
        float hotplatesInpact = 5.0f;

        // simulate the cooking process
        while (dishReadiness < 100)
        {
            Thread.Sleep(100);

            if (usedHotplates != 0) 
            {
                dishReadiness += usedHotplates * hotplatesInpact;

                Console.WriteLine($"{currentStudent} cooked the dish at {dishReadiness} percent");
            }

            // get the necessary freed burners
            usedHotplates += GetFreeHotplates(neededHotplates - usedHotplates);
        }

        // turning off the burners
        freeHotplates += usedHotplates;
    }
}

int GetFreeHotplates(int neededHotplates)
{
    if (neededHotplates == 0) return 0;

    int usedHotplates;

    if (freeHotplates <= neededHotplates)
    {
        usedHotplates = freeHotplates;
        freeHotplates = 0;
    }
    else
    {
        usedHotplates = neededHotplates;
        freeHotplates -= neededHotplates;
    }

    return usedHotplates;
}