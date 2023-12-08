using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoreLinq;

namespace Puzzle_2023_08_2
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

            var reg = new Regex(@"(\w+) = \((\w+), (\w+)\)");
            var lr = strings[0];
            var instructions = strings
                .Skip(2)
                .Select(s => reg.Match(s))
                .ToDictionary(m => m.Groups[1].Value, m => (m.Groups[2].Value, m.Groups[3].Value));
            var init = instructions.Keys.Where(k => k.EndsWith("A")).ToArray();
            var cycles = new List<(int start, int length, int[] zPos)>();
            for (int i = 0; i < init.Length; i++)
            {
                var curr = init[i];
                var list = new List<string>();
                list.Add(curr);
                var step = 0;
                var repeatStart = 0;
                while (repeatStart == 0)
                {
                    var left = lr[step % lr.Length] == 'L';
                    curr = left ? instructions[curr].Item1 : instructions[curr].Item2;
                    repeatStart = Enumerable
                        .Range(1, list.Count / lr.Length)
                        .Select(idx => list.Count - idx * lr.Length)
                        .Where(idx => list[idx] == curr)
                       .FirstOrDefault();
                    list.Add(curr);
                    step++;
                }
                var repeatLength = list.Count - repeatStart - 1;
                cycles.Add((repeatStart, repeatLength, Enumerable.Range(repeatStart, repeatLength).Where(i => list[i].EndsWith("Z")).Select(i => i - repeatStart).ToArray()));
            }

            var zPosCycles = cycles.Select(c => (c.start + c.zPos[0], c.length)).ToArray();
            var result = zPosCycles.Aggregate(1L, (s, v) => lcm(s, v.length));

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

        static long gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static long lcm(long a, long b)
        {
            return (a / gcf(a, b)) * b;
        }
    }
}