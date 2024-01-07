# Thread Management | [Code](../ExtraCodeExamples/ThreadManagement)

It's time to talk about thread synchronization. First of all, let's discuss one of the fundamental principles of synchronization - `locking`. For a
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

## Operator lock

How to resolve this situation then? Here is where the principle of thread synchronization - `locking` comes in handy. Its main purpose is to
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
the completion of other threads, thereby realizing the principle of synchronicity - `locking`.

## Class Monitor

We continue our exploration of thread synchronization, this time using the static class Monitor. It provides a wide range of useful tools that
allow for more flexible organization of thread work with critical sections.

Let's consider its main methods:

- **Monitor.Enter(object obj)** - Entry point into a critical section, which locks the object passed to the method for other threads. It works similarly
to the `lock` statement, where we pass an object - a lock. When the first thread visits the `Monitor.Enter(object obj)` method, the passed
object is locked, locking all other threads, and the thread that locked it continues its work.

- **Monitor.Exit(object obj)** - Exit point from the critical section, which unlocks the passed object, allowing another thread to enter the critical
section, locking this object again.

*Using the Monitor.Enter(object obj) and Monitor.Exit(object obj) methods in pair is crucial to avoid creating deadlocks (a state where one thread has
locked access to a critical section for other threads and hasn't unlocked it after completing all actions, thereby freezing the program execution).*

- **Monitor.TryEnter(object obj)** - A method that allows the current thread to check if the passed object is locked or not. Returns `true` if it is
free and`false` if it is locked.

- **Monitor.Pulse(object obj)** - Sends a signal to one of the threads in waiting, indicating that the current thread has finished its work and
unlocked the passed object.

- **Monitor.PulseAll(object obj)** - Sends a signal to all threads in waiting, indicating that the current thread has finished its work and unlocked
the passed object.

- **Monitor.Wait(object obj)** - Unlocks the passed object, puts the current thread into a queue, where it waits for a call to the
`Monitor.Pulse(object obj)` or `Monitor.PulseAll(object obj)` method to resume its work.

Let's recall our previous example of synchronization and refactor it using locking through the Monitor class:

```csharp
int freeHotplates = 4;
int averageHotplatesUse = 3;
int cookingTime;

// determine our glamorous apron
object apron = new();

// tell our students that it's time for lunch
for (int i = 1; i <= 4; i++)
{
    Thread student = new Thread(PrepareDishWhithMonitor);
    student.Name = $"Student {i}";
    student.Start(averageHotplatesUse);
}

void PrepareDishWhithMonitor(object? obj)
{
    if (obj is int neededHotplates)
    {

        // one student puts on an apron and starts cooking
        Monitor.Enter(apron);

        try
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
        // finish the job even when something went wrong
        finally
        {
            Monitor.Exit(apron);
            // student passed the apron to the next
        }
    }
}
```

Here, we replaced `lock(apron)` with `Monitor.Enter(apron)`, and wrapped the entire critical section in a try…finally block. In the finally block,
we called `Monitor.Exit(apron)` to ensure the unlocking of our lock object.

To make sure everything is working, let's run our simulation:

```console
Student 3 cooked the dish at 15 percent in 1 minutes
Student 3 cooked the dish at 30 percent in 2 minutes
Student 3 cooked the dish at 45 percent in 3 minutes
Student 3 cooked the dish at 60 percent in 4 minutes
Student 3 cooked the dish at 75 percent in 5 minutes
Student 3 cooked the dish at 90 percent in 6 minutes
Student 3 cooked the dish at 105 percent in 7 minutes
Student 1 cooked the dish at 15 percent in 1 minutes
Student 1 cooked the dish at 30 percent in 2 minutes
Student 1 cooked the dish at 45 percent in 3 minutes
Student 1 cooked the dish at 60 percent in 4 minutes
Student 1 cooked the dish at 75 percent in 5 minutes
Student 1 cooked the dish at 90 percent in 6 minutes
Student 1 cooked the dish at 105 percent in 7 minutes
Student 2 cooked the dish at 15 percent in 1 minutes
Student 2 cooked the dish at 30 percent in 2 minutes
Student 2 cooked the dish at 45 percent in 3 minutes
Student 2 cooked the dish at 60 percent in 4 minutes
Student 2 cooked the dish at 75 percent in 5 minutes
Student 2 cooked the dish at 90 percent in 6 minutes
Student 2 cooked the dish at 105 percent in 7 minutes
Student 4 cooked the dish at 15 percent in 1 minutes
Student 4 cooked the dish at 30 percent in 2 minutes
Student 4 cooked the dish at 45 percent in 3 minutes
Student 4 cooked the dish at 60 percent in 4 minutes
Student 4 cooked the dish at 75 percent in 5 minutes
Student 4 cooked the dish at 90 percent in 6 minutes
Student 4 cooked the dish at 105 percent in 7 minutes
```
We can see that the result hasn't changed, and everything is functioning correctly.

The use of the static Monitor class for thread synchronization is an important improvement compared to the regular `lock` statement, which is
essentially syntactic sugar for using this class. During compilation, `lock` is transformed into a similar structure to what we used in the above
example. However, due to the greater flexibility of the Monitor class, developers need to pay more attention to entry and exit points of the
critical section, carefully considering all possible execution scenarios to avoid deadlocks. In practice, in most cases, the functionality of the
`lock` statement is sufficient.

## AutoResetEvent

Another way to synchronize our threads is to use an instance of the `AutoResetEvent` class. It utilizes a signaling system that we can control
using a series of the following methods:

- **WaitOne()**: Blocks the calling thread until the event is signaled.
- **Set()**: Signals the event, releasing one waiting thread.
- **Reset()**: Manually resets the event to the non-signaled state.

When our instance is in a signaled state, it means that any thread can proceed with its work. However, when one of them reaches the `Reset()` call,
the object transitions to a nonsignaled state, blocking all threads. `WaitOne()` also blocks access to the critical section but allows the first
thread to proceed. After completion, it should change the non-signaled state to the opposite to avoid creating deadlocks.

Our example with using AutoResetEvent:

```csharp
int freeHotplates = 4;
int averageHotplatesUse = 3;

// determine the cooking time - our shared resources
int cookingTime;

// create an instance of AutoResetEvent
AutoResetEvent threadsHandler = new AutoResetEvent(true);

// tell our students that it's time for lunch
for (int i = 1; i <= 4; i++)
{
    Thread student = new Thread(PrepareDishWhithAutoResetEvent);
    student.Name = $"Student {i}";
    student.Start(averageHotplatesUse);
}

void PrepareDishWhithAutoResetEvent(object? obj)
{
    if (obj is int neededHotplates)
    {

        // one student puts on an apron and starts cooking
        threadsHandler.WaitOne();

        try
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
        // finish the job even when something went wrong
        finally
        {
            threadsHandler.Set();
            // student passed the apron to the next
        }
    }
}
...
```
If we compare the example using the `Monitor` class and `AutoResetEvent`, all we changed was to create an instance of the AutoResetEvent class, named
`threadsHandler`, removed the lock object `apron`, and replaced **Monitor.Enter(apron)** and **Monitor.Exit(apron)** with
**threadsHandler.WaitOne()** and **threadsHandler.Set()**, respectively. We kept the **try...finally** block to ensure thread release.

Result of the execution:

```console
Student 1 cooked the dish at 15 percent in 1 minutes
Student 1 cooked the dish at 30 percent in 2 minutes
Student 1 cooked the dish at 45 percent in 3 minutes
Student 1 cooked the dish at 60 percent in 4 minutes
Student 1 cooked the dish at 75 percent in 5 minutes
Student 1 cooked the dish at 90 percent in 6 minutes
Student 1 cooked the dish at 105 percent in 7 minutes
Student 2 cooked the dish at 15 percent in 1 minutes
Student 2 cooked the dish at 30 percent in 2 minutes
Student 2 cooked the dish at 45 percent in 3 minutes
Student 2 cooked the dish at 60 percent in 4 minutes
Student 2 cooked the dish at 75 percent in 5 minutes
Student 2 cooked the dish at 90 percent in 6 minutes
Student 2 cooked the dish at 105 percent in 7 minutes
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

## Conclusion

Now that you understand the principle of locking, let's summarize our knowledge and discuss recommendations for using the `lock` statement.

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
