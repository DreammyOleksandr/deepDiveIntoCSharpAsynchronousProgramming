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
