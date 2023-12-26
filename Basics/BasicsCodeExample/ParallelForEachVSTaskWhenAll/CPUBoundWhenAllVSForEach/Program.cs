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
