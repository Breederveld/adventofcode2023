using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using MoreLinq;

namespace Puzzle_2023_15_2
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

            var boxes = Enumerable.Range(0, 256).ToDictionary(i => i, i => new List<(string, int)>());
            foreach (var word in strings[0].Split(','))
            {
                if (word.EndsWith('-'))
                {
                    var label = word.Substring(0, word.Length - 1);
                    var hash = GetHashCode(label);
                    var list = boxes[hash];
                    list.RemoveAll(t => t.Item1 == label);
                }
                else
                {
                    var s = word.Split('=');
                    var hash = GetHashCode(s[0]);
                    var list = boxes[hash];
                    var items = list.Where(t => t.Item1 == s[0]).ToArray();
                    var index = list.Count;
                    foreach (var item in items)
                    {
                        index = boxes[hash].IndexOf(item);
                        boxes[hash].Remove(item);
                    }
                    list.Insert(index, (s[0], int.Parse(s[1])));
                }
            }
            var result = boxes.SelectMany(kv => kv.Value.Select((t, idx) => (kv.Key + 1) * (idx + 1) * t.Item2)).Sum();

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

        public static int GetHashCode([DisallowNull] string obj)
        {
            var hash = 0;
            foreach (var chr in obj)
            {
                hash = ((hash + (int)chr) * 17) % 256;
            }
            return hash;
        }
    }
}