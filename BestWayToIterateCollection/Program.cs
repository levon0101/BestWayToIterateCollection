
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BestWayToIterateCollection;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Starting");

        BenchmarkRunner.Run<BenchmarkTest>();


        Console.WriteLine("Finished");

        Console.ReadKey();
    }
}

[MemoryDiagnoser]
public class BenchmarkTest
{
    private static readonly Random Rng = new();
    private List<int> _items;

    [Params(100, 100_000, 1_000_000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _items = Enumerable.Range(1, Size).Select(x => Rng.Next()).ToList();
    }
    


    [Benchmark]
    public void Foreach_Test()
    {
        foreach (var item in _items)
        {

        }
    }


    [Benchmark]
    public void For_Test()
    {
        for (int i = 0; i < Size; i++)
        {
            var item = _items[i];
        }
    }


    [Benchmark]
    public void Foreach_Linq()
    {
        _items.ForEach(i => { });
    }


    [Benchmark]
    public void Parallel_Foreach()
    {
        Parallel.ForEach(_items, x => { });
    }


    [Benchmark]
    public void Parallel_Linq()
    {
        _items.AsParallel().ForAll(i => { });
    }

    [Benchmark]
    public void Foreach_span()
    {
        foreach (var item in CollectionsMarshal.AsSpan(_items))
        {

        }
    }

    [Benchmark]
    public void For_span()
    {
        var asSpan = CollectionsMarshal.AsSpan(_items);

        for (int i = 0; i < asSpan.Length; i++)
        {
            var item = asSpan[i];
        }
    }
}