using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2023_13_1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var rootFolder = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var input = File.ReadAllText(Path.Combine(rootFolder, "input.txt"));

            var sw = new Stopwatch();
            var strings = input.Trim().Split("\n").Select(s => s.TrimEnd()).ToArray();
            var groups = input.Trim().Split("\n\n").Select(grp => grp.Split("\n").ToArray()).ToArray();
            sw.Start();

            var vertical = groups
                .Select(group => Enumerable.Range(1, group[0].Length - 1)
                    .FirstOrDefault(x => Enumerable.Range(0, group.Length)
                        .All(y => Enumerable.Range(0, Math.Min(x, group[0].Length - x)).All(xx => group[y][x - xx - 1] == group[y][x + xx]))))
                .ToArray();
            var horizontal = groups
                .Select(group => Enumerable.Range(1, group.Length - 1)
                    .FirstOrDefault(y => Enumerable.Range(0, group[0].Length)
                        .All(x => Enumerable.Range(0, Math.Min(y, group.Length - y)).All(yy => group[y - yy - 1][x] == group[y + yy][x]))))
                .ToArray();

            var result = vertical.Sum() + horizontal.Sum() * 100;

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}