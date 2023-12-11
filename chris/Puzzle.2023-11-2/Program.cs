using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Puzzle_2023_11_2
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
            var galaxies = Enumerable.Range(0, width).SelectMany(x => Enumerable.Range(0, height).Select(y => (x, y)))
                .Where(p => strings[p.y][p.x] == '#')
                .ToArray();
            var expandX = Enumerable.Range(0, width).Where(i => !galaxies.Any(t => t.x == i)).ToArray();
            var expandY = Enumerable.Range(0, height).Where(i => !galaxies.Any(t => t.y == i)).ToArray();

            var mappedGalaxies = galaxies
                .Select(t => (x: t.x + ((long)expandX.Count(x => x < t.x) * 999999), y: t.y + ((long)expandY.Count(y => y < t.y) * 999999)))
                .ToArray();

            
            var result = 0L;
            for (var i = 0; i < mappedGalaxies.Length - 1; i++)
            {
                for (var j = i + 1; j < mappedGalaxies.Length; j++)
                {
                    result += Math.Abs(mappedGalaxies[i].x - mappedGalaxies[j].x) + Math.Abs(mappedGalaxies[i].y - mappedGalaxies[j].y);
                }
            }


            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}