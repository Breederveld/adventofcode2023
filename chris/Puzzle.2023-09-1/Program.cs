using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2023_09_1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var rootFolder = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var input = File.ReadAllText(Path.Combine(rootFolder, "input.txt"));

            var sw = new Stopwatch();
            var strings = input.Trim().Split("\n").Select(s => s.TrimEnd()).ToArray();
            var ints = strings.Where(st => !string.IsNullOrWhiteSpace(st)).Select(st => st.Split(' ').Select(str => long.Parse(str)).ToArray()).ToArray();
            sw.Start();

            var result = 0L;
            foreach (var list in ints)
            {
                var last = new List<long>();
                long[] diffs = list;
                last.Add(list.Last());
                do
                {
                    diffs = Enumerable.Range(0, diffs.Length - 1).Select(i => diffs[i + 1] - diffs[i]).ToArray();
                    last.Add(diffs.Last());
                }
                while (!diffs.All(d => d == 0));
                var next = last.Sum();
                result += next;
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}