using System;
using System.Diagnostics;

namespace ConsoleApp4
{
    enum GrowthType
    {
        Faster,
        Slower
    }
    class Program
    {
        static uint sz = 10000;
        static uint iterations = 10;
        static void Main(string[] args)
        {
            SetTestConfigurations();
            Console.WriteLine("Running the test please wait...");
            var testResult = RunTheTest();
            WriteResult(testResult);
        }
        static void SetTestConfigurations()
        {
            Console.Write($"Enter array size [default is {sz}](for defuault value press enter):");
            if(uint.TryParse(Console.ReadLine(),out var size))
            {
                sz = size;
            }
            Console.Write($"Enter iterations number [default is {iterations}](for defuault value press enter):");
            if (uint.TryParse(Console.ReadLine(), out var its))
            {
                iterations = its;
            }
        }
        static TestResult RunTheTest()
        {
            var result = new TestResult();
            var sw = new Stopwatch();
            for (int it = 0; it < iterations; ++it)
            {
                int[,] tab = new int[sz, sz];
                sw.Start();
                for (int i = 0; i < sz; ++i)
                {
                    for (int j = 0; j < sz; ++j)
                    {
                        tab[i, j] = 1;
                    }
                }
                sw.Stop();
                result.AverageByRowAccessElapsed += sw.Elapsed.TotalMilliseconds;
                sw.Reset();
                sw.Start();
                for (int i = 0; i < sz; ++i)
                {
                    for (int j = 0; j < sz; ++j)
                    {
                        tab[j, i] = 1;
                    }
                }
                sw.Stop();
                result.AverageByColumnAccessElapsed += sw.Elapsed.TotalMilliseconds;
                sw.Reset();
            }
            result.AverageByRowAccessElapsed /= iterations;
            result.AverageByColumnAccessElapsed /= iterations;
            return result;
        }
        static void WriteResult(TestResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            Console.WriteLine($"Sequential array access (row by row) average elapsed time for {iterations} iterations: {result.AverageByRowAccessElapsed}(ms)");
            Console.WriteLine($"Non-sequential array access (column by column) average elapsed time for {iterations} iterations: {result.AverageByColumnAccessElapsed}(ms)");
            var growthType = result.AverageByRowAccessElapsed < result.AverageByColumnAccessElapsed ? GrowthType.Faster : GrowthType.Slower;
            if (growthType == GrowthType.Faster)
                Console.ForegroundColor = ConsoleColor.Green;
            else Console.ForegroundColor = ConsoleColor.Red;
            var growth = Math.Abs(result.AverageByColumnAccessElapsed - result.AverageByRowAccessElapsed);
            var num = Math.Min(result.AverageByRowAccessElapsed, result.AverageByColumnAccessElapsed);
            var growthIndicator = growthType == GrowthType.Faster ? "faster" : "slower";
            Console.WriteLine($"Accessing the array in a row-by-row approach is {growthIndicator} by {growth / num * 100:0.##}%");
            Console.ReadLine();
        }
    }
}
