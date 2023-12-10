using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;
using MoreLinq.Extensions;

namespace Puzzle_2023_10_2
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

            // Build inital grid.
            var width = strings[0].Length;
            var height = strings.Length;
            var grid = new int[width, height];

            // Mark main loop and inner edge.
            var mainLoop = FindLoop(strings, startX, startY);
            var inner = 2;
            var x = 0;
            var y = 0;
            foreach (var step in mainLoop)
            {
                x = step.x;
                y = step.y;
                grid[x, y] = 1;
                (var xx, var yy) = Move(x, y, (step.dir + 2) % 4);
                for (var i = 0; i < 2; i++)
                {
                    var right = Move(xx, yy, (step.dir + 1) % 4);
                    if (right.x >= 0 && right.x < width && right.y >= 0 && right.y < height && grid[right.x, right.y] == 0)
                    {
                        grid[right.x, right.y] = inner;
                    }
                    (xx, yy) = Move(xx, yy, step.dir);
                }
            }

            // Find inner entities.
            var todo = new Queue<(int x, int y)>(Enumerable.Range(0, width).SelectMany(x => Enumerable.Range(0, height).Select(y => (x, y)))
                .Where(p => grid[p.x, p.y] == inner));
            var result = 0;
            while (todo.Any())
            {
                var next = todo.Dequeue();
                result++;
                var around = new[] { (x: -1, y: 0), (x: 0, y: -1), (x: 0, y: 1), (x: 1, y: 0) };
                foreach (var pos in around)
                {
                    x = next.x + pos.x;
                    y = next.y + pos.y;
                    if (x >= 0 && x < width && y >= 0 && y < height && grid[x, y] == 0)
                    {
                        grid[x, y] = inner;
                        todo.Enqueue((x, y));
                    }
                }
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

        private static ICollection<(int x, int y, int dir)> FindLoop(string[] strings, int startX, int startY)
        {
            var moves = new List<(int x, int y, int dir)>();

            var x = startX;
            var y = startY;
            var width = strings[0].Length;
            var height = strings.Length;
            var prev = (x, y);

            var dir = 0; // 0: up, 1: right, 2: down, 3: left
            do
            {
                var curr = (x, y);
                switch (strings[y][x])
                {
                    case 'S':
                        dir = 1; // Assumption
                        break;
                    case '|':
                        if (prev.y == y - 1)
                            dir = 2;
                        else
                            dir = 0;
                        break;
                    case '-':
                        if (prev.x == x - 1)
                            dir = 1;
                        else
                            dir = 3;
                        break;
                    case 'L':
                        if (prev.y == y - 1)
                            dir = 1;
                        else
                            dir = 0;
                        break;
                    case 'J':
                        if (prev.y == y - 1)
                            dir = 3;
                        else
                            dir = 0;
                        break;
                    case '7':
                        if (prev.y == y + 1)
                            dir = 3;
                        else
                            dir = 2;
                        break;
                    case 'F':
                        if (prev.y == y + 1)
                            dir = 1;
                        else
                            dir = 2;
                        break;
                    case '.':
                        return null;
                }
                prev = curr;
                (x, y) = Move(x, y, dir);
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    moves.Add((x, y, dir));
                }
                else
                {
                    return null;
                }

                // Failsafe for loops not originated in from the loop.
                if (moves.Count > 100000)
                {
                    return null;
                }
            }
            while (x != startX || y != startY);
            moves.Add((x, y, dir));

            return moves;
        }

        private static (int x, int y) Move(int x, int y, int dir)
        {
            switch (dir)
            {
                default:
                case 0: // up
                    return (x, y - 1);
                case 1: // right
                    return (x + 1, y);
                case 2: // down
                    return (x, y + 1);
                case 3: // left
                    return (x - 1, y);
            }
        }
    }
}
