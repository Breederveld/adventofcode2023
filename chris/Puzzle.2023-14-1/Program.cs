using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace Puzzle_2023_14_1
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

            var width = strings[0].Length;
            var height = strings.Length;
            var grid = new int[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var chr = strings[y][x];
                    grid[x, y] = chr == 'O' ? 1 : chr == '#' ? 2 : 0;
                }
            }

            var result = 0;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (grid[x, y] == 1)
                    {
                        grid[x, y] = 0;
                        var newY = Enumerable.Range(0, y + 1).Select(yy => y - yy + 1).SkipWhile(yy => grid[x, yy - 1] == 0).FirstOrDefault();
                        grid[x, newY] = 1;
                        result += height - newY;
                    }
                }
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}