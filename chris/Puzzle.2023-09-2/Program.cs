using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2023_09_2
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
                last.Add(list.First());
                do
                {
                    diffs = Enumerable.Range(0, diffs.Length - 1).Select(i => diffs[i + 1] - diffs[i]).ToArray();
                    last.Add(diffs.First());
                }
                while (!diffs.All(d => d == 0));
                last.Reverse();
                var next = last.Aggregate(0L, (s, i) => i - s);
                result += next;
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}