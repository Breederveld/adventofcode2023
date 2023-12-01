using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2023_01_2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var rootFolder = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var input = File.ReadAllText(Path.Combine(rootFolder, "input.txt"));

            var sw = new Stopwatch();
            var strings = input.Trim().Split("\n").Select(s => s.TrimEnd()).ToArray();
            sw.Start();

            var arr = new[] { "1", "one", "2", "two", "3", "three", "4", "four", "5", "five", "6", "six", "7", "seven", "8", "eight", "9", "nine" };
            var result = strings
                .Select(s =>
                {
                    var idx = Enumerable.Range(0, s.Length).First(i => arr.Any(a => s.Substring(i, s.Length - i).StartsWith(a)));
                    var ss = s.Substring(idx, s.Length - idx);
                    var first = arr.FindIndexes(a => ss.StartsWith(a)).First() / 2 + 1;
                    idx = Enumerable.Range(0, s.Length).First(i => arr.Any(a => s.Substring(s.Length - i - 1, i + 1).StartsWith(a)));
                    ss = s.Substring(s.Length - idx - 1, idx + 1);
                    var last = arr.FindIndexes(a => ss.StartsWith(a)).First() / 2 + 1;
                    return first * 10 + last;
                })
                .Sum();

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}