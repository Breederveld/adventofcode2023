using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzle_2023_08_1
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
            var result = 0;
            var curr = "AAA";
            while (curr != "ZZZ")
            {
                var left = lr[result % lr.Length] == 'L';
                curr = left ? instructions[curr].Item1 : instructions[curr].Item2;
                result++;
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}
