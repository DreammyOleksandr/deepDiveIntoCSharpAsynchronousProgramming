# Parallel.ForEach() VS Task.WhenAll() | [Code](../BasicsCodeExample/ParallelForEachVSTaskWhenAll/)

The purpose of this section is to determine how effective the use of **Task.WhenAll** and **Parallel.ForEachAsync** operations is in various situations
under different conditions. Here, we will address questions such as:
- which approach is better to use when dealing with I/O bound tasks, and which one is more suitable for CPU bound tasks?
- Which method is more scalable and, consequently, less responsive?
- How do these operations respond to an increase in task execution time?
- How do these methods respond to an increase in the number of tasks?

We will find answers to all these questions and demonstrate through simple examples where it is advisable to use **Task.WhenAll** and where to use **Parallel.ForEachAsync**.

For our grand testing, we'll need to install a specific package, `BenchmarkDotNet`, via the NuGet Package Manager.

Well, let's start our crazy comparison.

## Parallel.ForEach() VS Task.WhenAll() in I/O bound tasks | [Code](../BasicsCodeExample/ParallelForEachVSTaskWhenAll/IOBoundWhenAllVSForEach/Program.cs)

To simulate `input/output` tasks, we will use the method Task.Delay(). It will simulate a delay that might occur during reading from or writing to
external sources like files, databases, network resources, and so on.

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<IOBoundTaskBenchmarks>();
public class IOBoundTaskBenchmarks
{

    [Params(1, 10, 50, 100)]
    public int OperationCount;

    [Params(10, 100, 500, 1_000)]
    public int OperationDurationMs;

    [Benchmark]
    public async Task WenAll()
    {
        // creating an array of tasks
        var tasks = Enumerable.Range(0, OperationCount).Select(async i =>
        {
            // doing I/O Bound task
            await Task.Delay(OperationDurationMs);
        })
        .ToArray();

        // calling all tasks
        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task ParallelForEach()
    {
        // performing tasks for each collection item in parallel
        await Parallel.ForEachAsync(Enumerable.Range(0, OperationCount), async (item, _) =>
        {
            // doing I/O Bound task
            await Task.Delay(OperationDurationMs);
        });
    }
}

```

Let me explain a bit about what's happening here. In the WhenAll method, we first create an array of tasks that we want to invoke with
**Task.WhenAll**. To do this, we generated a sequence of integers from 0 to 99 (inclusive) and used the `Select` method to transform it into a set of
tasks, each taking the same amount of time. After that, we explicitly call our tasks in our **Task.WhenAll** method.

In the case of **Parallel.ForEachAsync**, the situation is almost identical. However, here we create a sequence of integers and pass it as the first
parameter of the method. As the second parameter, we pass a Func delegate, which takes each element of the collection sequentially and
performs an operation on it, simulating an I/O-bound task.

As we can see in both cases, we are performing the same operation the same number of times. However, let's examine the results of our
benchmark and determine which approach executed faster.

<p>
    <img src="./Parallel ForEach VS Task WhenAll Images/Benchmark for IO tasks.png" alt="WenAll Graph">
</p>

Here, we will focus our attention on the number of operations (`OperationCount`) and the average duration (`Mean`). From the obtained table,
we can see that until the number of operations reaches 100, the average duration of both methods is approximately the same and almost equals
the duration of a single operation. However, once the number of operations becomes 50 and above, we immediately notice a decrease in the
efficiency of **Parallel.ForEachAsync** execution by several times. As you may have already guessed, the increase in time is related to the number of
available threads that **Parallel.ForEachAsync** distributes the work of the given tasks to. In our case, we have 16 logical cores, so
**Parallel.ForEachAsync** can simultaneously process only 16 tasks, whereas **Task.WhenAll**, following the asynchronous principle, is capable
of processing all tasks simultaneously.

Let's consider a schematic representation of how these methods work:

## ParallelForEach()

<p>
    <img src="./Parallel ForEach VS Task WhenAll Images/ForEachGraph.png" alt="ParallelForEach Graph">
</p>

Since the duration of execution for all tasks is the same, the schematic representation will look like this: only 16 tasks are executed
simultaneously, and then another 16, and so on until all tasks are completed. It's worth noting that if the duration of each task were different,
they would still be processed by the same number of threads as in our case. The difference lies only in the fact that after completing one task,
the TPL (`Task Parallel Library`) would replace it with the next one in the queue, and so on until all tasks are completed.

The number of threads used by **Parallel.ForEach** is set by default and equals the number of logical cores, which is 16 in our case. To change the
number of threads for usage, you need to pass an options object as the second parameter, in which you can set the desired value for the
`MaxDegreeOfParallelism` property.

```csharp
var options = new ParallelOptions
{
    MaxDegreeOfParallelism = 4 // the desired number of threads
};

// put options into the method
Parallel.ForEach(Enumerable.Range(1, 100), options, i =>
{
    // your processing
});
```

## WenAll()

<p>
    <img src="./Parallel ForEach VS Task WhenAll Images/WhenAllGraph.png" alt="WenAll Graph">
</p>

Here, the same process occurs as described in the separate section about [Task.WhenAll](./TaskWhenAll.md).

Let's summarize briefly. So, we've noticed that tasks related to reading from or writing to external sources are executed approximately the same
when their quantity is small. However, when we have tens, hundreds, or thousands of such tasks that involve waiting for some delay,
**Task.WhenAll** has a clear advantage. In contrast to **Parallel.ForEachAsync**, which creates a thread pool and distributes tasks among them,
**Task.WhenAll** does not create new threads and does not wait for their launch. All it does is wait for the completion of all tasks. Therefore, we
recommend giving priority to the **Task.WhenAll** method, especially when dealing with I/O-bound tasks.

## Parallel.ForEach() VS Task.WhenAll() in CPU bound tasks | [Code](../BasicsCodeExample/ParallelForEachVSTaskWhenAll/CPUBoundWhenAllVSForEach/Program.cs)

For simulating `CPU-bound` tasks, we will, for example, perform simple mathematical operations. In our case, it will be squaring the current
iteration. In real-world scenarios, these could be any other operations related to mathematical calculations, sorting algorithms, matrix
operations, and so on.

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<CPUBoundTaskBenchmarks>();

public class CPUBoundTaskBenchmarks
{

    [Params(1, 10, 50, 100)]
    public int OperationCount;

    [Params(1_000, 10_000, 50_000, 100_000)]
    public int CalculationCount;

    [Benchmark]
    public async Task WenAll()
    {
        // creating an array of tasks
        var tasks = Enumerable.Range(0, OperationCount).Select(_ =>
        {
            // computationally intensive task
            for (int i = 0; i < CalculationCount; i++)
            {
                Math.Pow(i, 2);
            }
            return Task.CompletedTask;
        })
        .ToArray();

        // calling all tasks
        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task ParallelForEach()
    {
        // performing tasks for each collection item in parallel
        await Parallel.ForEachAsync(Enumerable.Range(0, OperationCount), (item, _) =>
        {
            // computationally intensive task
            for (int i = 0; i < CalculationCount; i++)
            {
                Math.Pow(i, 2);
            }
            return ValueTask.CompletedTask;
        });
    }
}

```

The code closely resembles the previous case and differs only in the fact that instead of artificially introducing a delay through `Task.Delay()`, we
perform a mathematical operation `Math.Pow(i, 2)` a large number of times. Again, **Task.WhenAll()** and **Parallel.ForEach()** are absolutely identical
in terms of the tasks they need to perform.

Well, let's run our benchmark again to see what the results will be this time.

<p>
    <img src="./Parallel ForEach VS Task WhenAll Images/Benchmark for CPU tasks.png" alt="WenAll Graph">
</p>

Now, this table shows us quite different and interesting results. In terms of the number of tasks and mathematical computations, we can
observe that Task.WhenAll is faster with a relatively small number of calculations. However, as the number of operations and mathematical
computations increases, everything turns the other way around. We can see that already with 10,000 calculations and 10+ tasks, the
**Parallel.ForEach()** method significantly outperforms, and with an increase in these calculations, the difference becomes even more pronounced.

Why did this happen? Well, the main reason is that we are dealing directly with the processor's work, not just waiting for a result. Here,
**Parallel.ForEachAsync** unleashes the potential of its created threads, where each thread can independently perform a mathematical operation.
On the other hand, **Task.WhenAll** will execute everything in a single thread, causing the time spent on calculations to be significantly longer.
Therefore, we conclude that when working with a large number of mathematical calculations, sorting algorithms, matrix operations, it is much
more efficient to use the **Parallel.ForEachAsync** method.

The working schemes of these methods will be almost identical to those discussed earlier, with the exception of the internal operations carried
out by these methods.

## Conclusion

As always, let's summarize the results of our research.

What we have learned:
- When working with a large number of I/O-bound tasks, the most efficient method is **Task.WhenAll**. It doesn't create unnecessary threads; it simply launches tasks and waits for their completion.
- When working with a large number of CPU-bound tasks, the most efficient method is **Parallel.ForEachAsync**, which allows you to utilize the full potential of your processor and significantly accelerate the execution speed of computational tasks.
- With a relatively small number of both I/O-bound and CPU-bound tasks, the methods work approximately the same, with a slight advantage towards **Task.WhenAll**.
- We can control the number of threads created by the **Parallel.ForEachAsync** operation for more optimal resource utilization.

Thank you for your time spent on this material. Stay with us and dive even deeper into the realms of asynchronous programming. Good luck.