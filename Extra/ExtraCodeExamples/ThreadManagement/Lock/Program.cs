int freeHotplates = 4;
int averageHotplatesUse = 3;

// determine the cooking time - our shared resources
int cookingTime;

// determine our glamorous apron
object apron = new();

// tell our students that it's time for lunch
for (int i = 1; i <= 4; i++)
{
    Thread student = new Thread(PrepareDishWithoutLock);
    student.Name = $"Student {i}";
    student.Start(averageHotplatesUse);
}

void PrepareDishWithoutLock(object? obj)
{
    if (obj is int neededHotplates)
    {
        // all students try to cook a dish

        string currentStudent = Thread.CurrentThread.Name!;
        int usedHotplates = GetFreeHotplates(neededHotplates);
        float dishReadiness = 0;
        float hotplatesInpact = 5.0f;

        cookingTime = 0;

        // simulating the cooking process.
        while (dishReadiness < 100)
        {
            Thread.Sleep(100);

            dishReadiness += usedHotplates * hotplatesInpact;
            cookingTime++;

            Console.WriteLine($"{currentStudent} cooked the dish at {dishReadiness} percent in {cookingTime} minutes");

            // acquiring the necessary available burners
            usedHotplates += GetFreeHotplates(neededHotplates - usedHotplates);
        }

        // turning off the burners
        freeHotplates += usedHotplates;
    }
}

void PrepareDishWhithLock(object? obj)
{
    if (obj is int neededHotplates)
    {
        // one student puts on an apron and starts cooking
        lock (apron)
        {
            string currentStudent = Thread.CurrentThread.Name!;
            int usedHotplates = GetFreeHotplates(neededHotplates);
            float dishReadiness = 0;
            float hotplatesInpact = 5.0f;

            cookingTime = 0;

            // simulating the cooking process.
            while (dishReadiness < 100)
            {
                Thread.Sleep(100);

                dishReadiness += usedHotplates * hotplatesInpact;
                cookingTime++;

                Console.WriteLine($"{currentStudent} cooked the dish at {dishReadiness} percent in {cookingTime} minutes");

                // acquiring the necessary available burners
                usedHotplates += GetFreeHotplates(neededHotplates - usedHotplates);
            }

            // turning off the burners
            freeHotplates += usedHotplates;
        }
        // student finished cooking and passed the apron to the next
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