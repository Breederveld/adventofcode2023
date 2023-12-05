using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle_2023_05_1
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

            var seeds = strings[0].Split(": ")[1].Split(' ').Select(s => long.Parse(s)).ToArray();
            var maps = new Dictionary<string, List<Map>>();
            var map = (string)null;
            foreach (var s in strings.Skip(2))
            {
                if (map == null)
                {
                    map = s.Split(' ')[0];
                    maps[map] = new List<Map>();
                    continue;
                }
                if (string.IsNullOrEmpty(s))
                {
                    map = null;
                    continue;
                }
                var ss = s.Split(' ');
                maps[map].Add(new Map(long.Parse(ss[1]), long.Parse(ss[0]), long.Parse(ss[2])));
            }

            var doMap = new Func<long, List<Map>, long>((num, map) =>
            {
                var match = map.FirstOrDefault(m => m.SourceStart <= num && m.SourceStart + m.Range >= num);
                return match == null ? num : num + match.TargetStart - match.SourceStart;
            });

            var result = seeds
                .Select(num => doMap(doMap(doMap(doMap(doMap(doMap(doMap(num, maps["seed-to-soil"]), maps["soil-to-fertilizer"]), maps["fertilizer-to-water"]), maps["water-to-light"]), maps["light-to-temperature"]), maps["temperature-to-humidity"]), maps["humidity-to-location"]))
                .Min();

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

				private record Map (long SourceStart, long TargetStart, long Range);
    }
}
