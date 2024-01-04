# Lock operator | [Code](../ExtraCodeExamples/Lock/Program.cs)

It's time to talk about thread synchronization. First of all, let's discuss one of the fundamental principles of synchronization - `blocking`. For a
better understanding, let's provide a real-life example that will help you fully grasp this concept.

Imagine a situation where we have a certain number of students living in a dormitory with one kitchen equipped with a stove with four burners.
On average, a student needs 3 burners to prepare a meal. Let's represent this in code as follows:

```csharp
int freeHotplates = 4;
int averageHotplatesUse = 3;
```

Let's also introduce a global variable where we will store the cooking time for one student. This variable will also be our shared resource, along
with the ones mentioned earlier.

```csharp
int cookingTime;
```

Now let's consider a situation where lunchtime has arrived, and a certain number of students are getting ready to cook their meals. In our
example, the students are represented by threads that execute a method simulating the cooking process.

```csharp
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
```

If several students are in the kitchen at once, using the same stove, it will lead to complete chaos. Everyone will want to occupy all burners for 
themselves, arguments will arise, some won't be able to fully cook their meals as they need multiple burners simultaneously, and others might 
not even get access to the stove at all. In the code, this would manifest as each thread constantly modifying shared resources, leading to
unpredictable results in the execution of methods.

Let's run our example and see the result:

```console
Student 1 cooked the dish at 15 percent in 1 minutes
Student 2 cooked the dish at 5 percent in 1 minutes
Student 3 cooked the dish at 0 percent in 1 minutes
Student 4 cooked the dish at 0 percent in 1 minutes
Student 1 cooked the dish at 30 percent in 3 minutes
Student 4 cooked the dish at 0 percent in 3 minutes
Student 3 cooked the dish at 0 percent in 3 minutes
Student 2 cooked the dish at 10 percent in 3 minutes
Student 1 cooked the dish at 45 percent in 4 minutes
Student 4 cooked the dish at 0 percent in 5 minutes
Student 2 cooked the dish at 15 percent in 4 minutes
Student 3 cooked the dish at 0 percent in 4 minutes
Student 2 cooked the dish at 20 percent in 7 minutes
Student 4 cooked the dish at 0 percent in 7 minutes
Student 1 cooked the dish at 60 percent in 7 minutes
Student 3 cooked the dish at 0 percent in 7 minutes
Student 3 cooked the dish at 0 percent in 11 minutes
Student 2 cooked the dish at 25 percent in 11 minutes
Student 4 cooked the dish at 0 percent in 11 minutes
Student 1 cooked the dish at 75 percent in 11 minutes
Student 3 cooked the dish at 0 percent in 15 minutes
Student 4 cooked the dish at 0 percent in 15 minutes
Student 2 cooked the dish at 30 percent in 15 minutes
Student 1 cooked the dish at 90 percent in 15 minutes
Student 1 cooked the dish at 105 percent in 19 minutes
Student 4 cooked the dish at 0 percent in 19 minutes
Student 3 cooked the dish at 0 percent in 19 minutes
Student 2 cooked the dish at 35 percent in 19 minutes
Student 3 cooked the dish at 0 percent in 22 minutes
Student 2 cooked the dish at 40 percent in 22 minutes
Student 4 cooked the dish at 15 percent in 22 minutes
Student 3 cooked the dish at 0 percent in 25 minutes
Student 2 cooked the dish at 45 percent in 25 minutes
Student 4 cooked the dish at 30 percent in 25 minutes
Student 2 cooked the dish at 50 percent in 28 minutes
Student 3 cooked the dish at 0 percent in 28 minutes
Student 4 cooked the dish at 45 percent in 28 minutes
Student 3 cooked the dish at 0 percent in 31 minutes
Student 2 cooked the dish at 55 percent in 31 minutes
Student 4 cooked the dish at 60 percent in 31 minutes
Student 4 cooked the dish at 75 percent in 34 minutes
Student 2 cooked the dish at 60 percent in 34 minutes
Student 3 cooked the dish at 0 percent in 34 minutes
Student 2 cooked the dish at 65 percent in 36 minutes
Student 3 cooked the dish at 0 percent in 36 minutes
Student 4 cooked the dish at 90 percent in 36 minutes
Student 4 cooked the dish at 105 percent in 37 minutes
Student 2 cooked the dish at 70 percent in 37 minutes
Student 3 cooked the dish at 0 percent in 37 minutes
Student 2 cooked the dish at 85 percent in 39 minutes
Student 3 cooked the dish at 5 percent in 39 minutes
Student 2 cooked the dish at 100 percent in 41 minutes
Student 3 cooked the dish at 10 percent in 41 minutes
Student 3 cooked the dish at 25 percent in 42 minutes
Student 3 cooked the dish at 40 percent in 43 minutes
Student 3 cooked the dish at 55 percent in 44 minutes
Student 3 cooked the dish at 70 percent in 45 minutes
Student 3 cooked the dish at 85 percent in 46 minutes
Student 3 cooked the dish at 100 percent in 47 minutes
```

Here it immediately becomes noticeable that the time for cooking is not at all what we expected. Even the students who haven't started
cooking yet have already spent some time on preparing the dish. The issue here is that in the example, all our threads simultaneously use and 
modify one global variable, `cookingTime`. This leads to the incorrect operation of our entire simulation and inaccurate cooking time 
representation.

How to resolve this situation then? Here is where the principle of thread synchronization - `blocking` comes in handy. Its main purpose is to
restrict access to shared resources when they are being used by any other thread. To implement this principle, we will use the `lock` keyword,
which indicates that the next code block will be restricted for use.

Let's go back to our example and adapt it to synchronization through locking. So, the cooking process can be organized in a way that only the
student who wears the unique, glamorous apron in the dormitory can use the stove. When a student puts on the apron, it means he has
exclusive access to the burners. Therefore, no one will disturb him, and he can calmlyprepare his meal. After finishing, he'll pass the apron to 
someone else.

Let's introduce our apron into the code and define the critical section (the part of the code that will have restricted access and where shared
resources are used). We'll wrap it in a code block, preceding it with the `lock` statement, into which we'll place our apron. This way, this code
block will be executed only by the thread that first acquires the `apron` object. After that, this object is locked for other threads, preventing
them from proceeding further.

```csharp
int freeHotplates = 4;
int averageHotplatesUse = 3;
int cookingTime;

// determine our glamorous apron
object apron = new();

// tell our students that it's time for lunch
for (int i = 1; i <= 4; i++)
{
    Thread student = new Thread(PrepareDishWhithLock); // here we put method with lock operator
    student.Name = $"Student {i}";
    student.Start(averageHotplatesUse);
}

void PrepareDishWhithLock(object? obj)
{
    if (obj is int neededHotplates)
    {
        // one student puts on an apron and starts cooking
        // start of critical section
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
        // end of critical section
        // student finished cooking and passed the apron to the next
    }
}
```

After preparing the dish, the student who cooked passes the apron to another student, and so on until everyone has cooked their lunch. This
way, we have managed to avoid chaos in the kitchen and allowed each student to peacefully prepare their meal.

Let's run our simulation again and see what the result will be this time.

```console
Student 2 cooked the dish at 15 percent in 1 minutes
Student 2 cooked the dish at 30 percent in 2 minutes
Student 2 cooked the dish at 45 percent in 3 minutes
Student 2 cooked the dish at 60 percent in 4 minutes
Student 2 cooked the dish at 75 percent in 5 minutes
Student 2 cooked the dish at 90 percent in 6 minutes
Student 2 cooked the dish at 105 percent in 7 minutes
Student 1 cooked the dish at 15 percent in 1 minutes
Student 1 cooked the dish at 30 percent in 2 minutes
Student 1 cooked the dish at 45 percent in 3 minutes
Student 1 cooked the dish at 60 percent in 4 minutes
Student 1 cooked the dish at 75 percent in 5 minutes
Student 1 cooked the dish at 90 percent in 6 minutes
Student 1 cooked the dish at 105 percent in 7 minutes
Student 3 cooked the dish at 15 percent in 1 minutes
Student 3 cooked the dish at 30 percent in 2 minutes
Student 3 cooked the dish at 45 percent in 3 minutes
Student 3 cooked the dish at 60 percent in 4 minutes
Student 3 cooked the dish at 75 percent in 5 minutes
Student 3 cooked the dish at 90 percent in 6 minutes
Student 3 cooked the dish at 105 percent in 7 minutes
Student 4 cooked the dish at 15 percent in 1 minutes
Student 4 cooked the dish at 30 percent in 2 minutes
Student 4 cooked the dish at 45 percent in 3 minutes
Student 4 cooked the dish at 60 percent in 4 minutes
Student 4 cooked the dish at 75 percent in 5 minutes
Student 4 cooked the dish at 90 percent in 6 minutes
Student 4 cooked the dish at 105 percent in 7 minutes
```
*In this case, we see that the second student started first, followed by the first, third, and fourth. This is due to the peculiarities of thread execution,
and the result may vary for each run.*

Now we see that the students are cooking their meals one by one, and the time they spend on cooking is displayed correctly. Thus, by using
the `lock` statement, we have made our code synchronous and avoided unpredictable changes to global variables. We made our threads wait for 
the completion of other threads, thereby realizing the principle of synchronicity - `blocking`.

## Conclusion

Now that you understand the principle of blocking, let's summarize our knowledge and discuss recommendations for using the `lock` statement.

The `lock` statement is used to ensure that a block of code runs in a mutually exclusive manner, meaning that only one thread can execute the
code block at a time. This is particularly useful for preventing race conditions and ensuring proper synchronization when multiple threads are
accessing shared resources.

Our recommendations for using the lock statement:

1. **Use `lock` when multiple threads need to access shared data safely**
2. **Keep critical sections short**
      - Minimize the amount of work performed within a `lock` statement to reduce the time the lock is held. Long-running critical sections can increase the likelihood of contention and reduce overall performance.
3. **Avoid Deadlocks**
      - Be cautious of nested locks and ensure that the locks are acquired and released in the same order across different parts of your code. Deadlocks can occur when multiple threads are waiting for each other to release a `lock`.
4. **Avoid Locking on Public Objects**
      - Locking on public objects or types might lead to unintended consequences and can create opportunities for deadlocks if other code also locks on the same objects.
