using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2023_04_2
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
            var sums = cards
                .Select(c => c[0].Where(cc => c[1].Contains(cc)).Count())
                .ToArray();
            var counts = Enumerable.Range(0, sums.Length).Select(i => (Int64)1).ToArray();
            for (var i = 0; i < sums.Length; i++)
            {
                var t = counts[i];
                for (int ii = 0; ii < sums[i]; ii++)
                {
                    if (i + ii >= sums.Length)
                        continue;
                    counts[i + ii + 1] += t;
                }
            }
            var result = counts.Sum();

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}