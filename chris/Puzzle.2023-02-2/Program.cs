using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2023_02_2
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

            var games = strings
                .Select(s => s.Split(": ")[1].Split(';').Select(ss => ss.Split(',').ToDictionary(sss => sss.Trim().Split(' ')[1].Trim(), sss => int.Parse(sss.Trim().Split(' ')[0].Trim()))).ToArray()).ToArray()
                .ToArray();
            var result = Enumerable.Range(0, games.Length)
                .Sum(i => games[i].Max(d => d.ContainsKey("red") ? d["red"] : 0) * games[i].Max(d => d.ContainsKey("green") ? d["green"] : 0) * games[i].Max(d => d.ContainsKey("blue") ? d["blue"] : 0));

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}