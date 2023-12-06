using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2023_06_1
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

            var times = strings[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
            var distances = strings[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
            var getDistance = new Func<int, int, int>((duration, button) => (duration - button) * button);
            var result = 1;
            for (int i = 0; i < times.Length; i++)
            {
                var time = times[i];
                var distance = distances[i];
                var wins = Enumerable.Range(0, time + 1)
                    .Where(button => getDistance(time, button) > distance)
                    .Count();
                result *= wins;
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}