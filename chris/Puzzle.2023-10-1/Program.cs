using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace Puzzle_2023_10_1
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

            var startY = Enumerable.Range(0, strings.Length).First(i => strings[i].Contains('S'));
            var startX = strings[startY].IndexOf('S');

            var steps = new List<(int x, int y)>();
            steps.Add((startX, startY));

            var x = startX;
            var y = startY;
            var prev = (x, y);

            y--;
            steps.Add((x, y));
            while (x != startX || y != startY)
            {
                var curr = (x, y);
                switch (strings[y][x])
                {
                    case '|':
                        if (prev.y == y - 1)
                            y++;
                        else
                            y--;
                        break;
                    case '-':
                        if (prev.x == x - 1)
                            x++;
                        else
                            x--;
                        break;
                    case 'L':
                        if (prev.y == y - 1)
                            x++;
                        else
                            y--;
                        break;
                    case 'J':
                        if (prev.y == y - 1)
                            x--;
                        else
                            y--;
                        break;
                    case '7':
                        if (prev.y == y + 1)
                            x--;
                        else
                            y++;
                        break;
                    case 'F':
                        if (prev.y == y + 1)
                            x++;
                        else
                            y++;
                        break;
                }
                prev = curr;
                steps.Add((x, y));
            }

            var result = steps.Count / 2;

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}