using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2023_07_2
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

            var hands = strings.Select(s => s.Split(' ')).ToArray();
            var comparer = new HandComparer();
            hands = hands.OrderBy(h => h[0], comparer).ToArray();
            var result = Enumerable.Range(0, hands.Length).Sum(i => (i + 1) * int.Parse(hands[i][1]));

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

        private class HandComparer : IComparer<string>
        {
            private static string _values = "J23456789TQKA";

            public int Compare(string x, string y)
            {
                var jokersX = x.Count(c => c == 'J');
                var countsX = x
                    .Where(c => c != 'J')
                    .GroupBy(c => c)
                    .Select(grp => grp.Count())
                    .OrderByDescending(cnt => cnt)
                    .ToArray();
                if (countsX.Length == 0)
                    countsX = new[] { 0 };
                countsX[0] += jokersX;
                var jokersY = y.Count(c => c == 'J');
                var countsY = y
                    .Where(c => c != 'J')
                    .GroupBy(c => c)
                    .Select(grp => grp.Count())
                    .OrderByDescending(cnt => cnt)
                    .ToArray();
                if (countsY.Length == 0)
                    countsY = new[] { 0 };
                countsY[0] += jokersY;
                var cmp = Enumerable.Range(0, Math.Min(countsX.Length, countsY.Length))
                    .Select(i => countsX[i] == countsY[i] ? 0 : countsX[i] > countsY[i] ? 1 : -1)
                    .Where(cmp => cmp != 0)
                    .FirstOrDefault();
                if (cmp != 0)
                {
                    return cmp;
                }
                return Enumerable.Range(0, Math.Min(x.Length, y.Length))
                    .Select(i => _values.IndexOf(x[i]).CompareTo(_values.IndexOf(y[i])))
                    .Where(cmp => cmp != 0)
                    .FirstOrDefault();
            }
        }
    }
}