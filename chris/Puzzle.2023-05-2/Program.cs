using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;

namespace Puzzle_2023_05_2
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
            var seedRanges = Enumerable.Range(0, seeds.Length / 2).Select(i => new Map(seeds[i * 2], seeds[i * 2], seeds[i * 2 + 1])).ToList();
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

            foreach (var key in maps.Keys)
            {
                var list = maps[key];

                // Add normal mapping.
                list.Add(new Map(0, 0, long.MaxValue));

                // Normalize mappings (remove overlapping ranges).
                var newList = new List<Map>();
                for (int i = 0; i < list.Count; i++)
                {
                    var currList = list[i];
                    var overlaps = Enumerable.Range(0, i)
                        .Select(ii => list[ii])
                        .Where(prevList => prevList.SourceStart <= currList.SourceStart + currList.Range && prevList.SourceStart + prevList.Range >= currList.SourceStart)
                        .ToArray();
                    if (!overlaps.Any())
                    {
                        newList.Add(currList);
                        continue;
                    }
                    else
                    {
                        var offset = currList.TargetStart - currList.SourceStart;
                        var start = currList.SourceStart;
                        while (start < currList.SourceStart + currList.Range)
                        {
                            var nextStart = overlaps
                                .Where(prevList => prevList.SourceStart + prevList.Range > start)
                                .OrderBy(prevList => prevList.SourceStart)
                                .FirstOrDefault();
                            if (nextStart == null)
                            {
                                newList.Add(new Map(start, start + offset, currList.SourceStart + currList.Range - start));
                                break;
                            }
                            else if (nextStart.SourceStart > start)
                            {
                                newList.Add(new Map(start, start + offset, nextStart.SourceStart - start));
                            }
                            start = nextStart.SourceStart + nextStart.Range;
                        }
                    }
                    maps[key] = newList;
                }
            }

            var doMap = new Func<List<Map>, List<Map>, List<Map>>((source, map) =>
            {
                var mapped = new List<Map>();
                foreach (var s in source)
                {
                    foreach (var m in map.OrderBy(m => m.SourceStart))
                    {
                        var targetStart = Math.Max(s.TargetStart, m.SourceStart);
                        var targetEnd = Math.Min(s.TargetStart + s.Range, m.SourceStart + m.Range);
                        var range = targetEnd - targetStart;
                        if (range <= 0)
                        {
                            continue;
                        }
                        var targetOffset = m.TargetStart - m.SourceStart;
                        var sourceOffset = s.TargetStart - s.SourceStart;
                        mapped.Add(new Map(targetStart - sourceOffset, targetStart + targetOffset, range));
                    }
                }
                return mapped;
            });

            var ranges = seedRanges;
            ranges = doMap(ranges, maps["seed-to-soil"]);
            ranges = doMap(ranges, maps["soil-to-fertilizer"]);
            ranges = doMap(ranges, maps["fertilizer-to-water"]);
            ranges = doMap(ranges, maps["water-to-light"]);
            ranges = doMap(ranges, maps["light-to-temperature"]);
            ranges = doMap(ranges, maps["temperature-to-humidity"]);
            ranges = doMap(ranges, maps["humidity-to-location"]);
            var result = ranges
                .Min(range => range.TargetStart);

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

		private record Map (long SourceStart, long TargetStart, long Range);
    }
}
