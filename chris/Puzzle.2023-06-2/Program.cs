using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2023_06_2
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

            var time = long.Parse(strings[0].Split(':')[1].Replace(" ", ""));
            var distance = long.Parse(strings[1].Split(':')[1].Replace(" ", ""));
            var getDistance = new Func<long, long, long>((duration, button) => (duration - button) * button);
            var result = 0;
            for (long button = 0; button < time; button++)
            {
                if (getDistance(time, button) > distance)
                {
                    result++;
                }
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}