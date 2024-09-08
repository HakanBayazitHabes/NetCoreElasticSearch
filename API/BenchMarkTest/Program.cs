// Benchmark programı
using BenchmarkDotNet.Running;
using BenchMarkTest;

class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<DrugBarcodeBenchmark>();
        }
    }