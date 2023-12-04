using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2023_04_1
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

            var cards = strings
                .Select(s => s.Split(": ")[1])
                .Select(s => s.Split(" | ").Select(ss => ss.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(sss => int.Parse(sss.Trim())).ToArray()).ToArray())
                .ToArray();
            var result = cards
                .Select(c => c[0].Where(cc => c[1].Contains(cc)).Count())
                .Select(c => c == 0 ? 0 : Math.Pow(2, c - 1))
                .Sum();

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}
