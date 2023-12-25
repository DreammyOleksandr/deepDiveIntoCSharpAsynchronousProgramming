# Parallel.ForEach() VS Task.WhenAll() | Code

The purpose of this section is to determine how effective the use of Task.WhenAll and Parallel.ForEachAsync operations is in various situations
under different conditions. Here, we will address questions such as:
- which approach is better to use when dealing with I/O bound tasks, and which one is more suitable for CPU bound tasks?
- Which method is more scalable and, consequently, less responsive?
- How do these operations respond to an increase in task execution time?

We will find answers to all these questions and demonstrate through simple examples where it is advisable to use Task.WhenAll and where to use Parallel.ForEachAsync.

For our grand testing, we'll need to install a specific package, `BenchmarkDotNet`, via the NuGet Package Manager.

Well, let's start our crazy comparison.

## Parallel.ForEach() VS Task.WhenAll() in I/O bound tasks | Code

To simulate input/output tasks, we will use the method Task.Delay(). It will simulate a delay that might occur during reading from or writing to
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
Task.WhenAll. To do this, we generated a sequence of integers from 0 to 99 (inclusive) and used the Select method to transform it into a set of
tasks, each taking the same amount of time. After that, we explicitly call our tasks in our Task.WhenAll method.

In the case of Parallel.ForEachAsync, the situation is almost identical. However, here we create a sequence of integers and pass it as the first
parameter of the method. As the second parameter, we pass a Func delegate, which takes each element of the collection sequentially and
performs an operation on it, simulating an I/O-bound task.

As we can see in both cases, we are performing the same operation the same number of times. However, let's examine the results of our
benchmark and determine which approach executed faster.
```console
| Method          | OperationCount | OperationDurationMs | Mean        | Error     | StdDev    |
|---------------- |--------------- |-------------------- |------------:|----------:|----------:|
| WenAll          | 1              | 10                  |    15.53 ms |  0.038 ms |  0.036 ms |
| ParallelForEach | 1              | 10                  |    15.56 ms |  0.041 ms |  0.038 ms |
| WenAll          | 1              | 100                 |   108.78 ms |  0.258 ms |  0.242 ms |
| ParallelForEach | 1              | 100                 |   108.71 ms |  0.234 ms |  0.219 ms |
| WenAll          | 1              | 500                 |   512.49 ms |  1.570 ms |  1.469 ms |
| ParallelForEach | 1              | 500                 |   510.48 ms |  4.481 ms |  4.191 ms |
| WenAll          | 1              | 1000                | 1,011.50 ms |  3.321 ms |  3.107 ms |
| ParallelForEach | 1              | 1000                | 1,010.75 ms |  1.760 ms |  1.561 ms |
| WenAll          | 10             | 10                  |    15.58 ms |  0.035 ms |  0.031 ms |
| ParallelForEach | 10             | 10                  |    15.62 ms |  0.045 ms |  0.040 ms |
| WenAll          | 10             | 100                 |   108.94 ms |  0.486 ms |  0.454 ms |
| ParallelForEach | 10             | 100                 |   109.07 ms |  0.575 ms |  0.538 ms |
| WenAll          | 10             | 500                 |   510.08 ms |  5.708 ms |  5.339 ms |
| ParallelForEach | 10             | 500                 |   511.38 ms |  4.023 ms |  3.763 ms |
| WenAll          | 10             | 1000                | 1,009.59 ms |  4.003 ms |  3.744 ms |
| ParallelForEach | 10             | 1000                | 1,011.62 ms |  2.267 ms |  2.121 ms |
| WenAll          | 50             | 10                  |    15.56 ms |  0.023 ms |  0.022 ms |
| ParallelForEach | 50             | 10                  |    62.14 ms |  0.248 ms |  0.232 ms |
| WenAll          | 50             | 100                 |   108.69 ms |  0.335 ms |  0.313 ms |
| ParallelForEach | 50             | 100                 |   434.51 ms |  1.504 ms |  1.407 ms |
| WenAll          | 50             | 500                 |   509.65 ms |  4.415 ms |  4.130 ms |
| ParallelForEach | 50             | 500                 | 2,046.81 ms |  8.301 ms |  7.764 ms |
| WenAll          | 50             | 1000                | 1,011.28 ms |  1.723 ms |  1.612 ms |
| ParallelForEach | 50             | 1000                | 4,034.38 ms |  5.783 ms |  5.410 ms |
| WenAll          | 100            | 10                  |    15.49 ms |  0.053 ms |  0.050 ms |
| ParallelForEach | 100            | 10                  |   108.01 ms |  0.308 ms |  0.288 ms |
| WenAll          | 100            | 100                 |   108.11 ms |  0.444 ms |  0.415 ms |
| ParallelForEach | 100            | 100                 |   757.57 ms |  2.486 ms |  2.325 ms |
| WenAll          | 100            | 500                 |   510.49 ms |  2.331 ms |  2.180 ms |
| ParallelForEach | 100            | 500                 | 3,576.69 ms | 13.644 ms | 12.763 ms |
| WenAll          | 100            | 1000                | 1,011.43 ms |  2.019 ms |  1.889 ms |
| ParallelForEach | 100            | 1000                | 7,066.67 ms | 17.588 ms | 16.451 ms |
```
Here, we will focus our attention on the number of operations (OperationCount) and the average duration (Mean). From the obtained table,
we can see that until thenumber of operations reaches 100, the average duration of both methods is approximately the same and almost equals
the duration of a single operation. However, once the number of operations becomes 50 and above, we immediately notice a decrease in the
efficiency of Parallel.ForEachAsync execution by several times.As you may have already guessed, the increase in time is related to the number of
available threads that Parallel.ForEachAsync distributes the work of the given tasks to. In our case, we have 16 logical cores, so
Parallel.ForEachAsync can simultaneously process only 16 tasks, whereas Task.WhenAll, following the asynchronous principle, is capable
of processing all tasks simultaneously.