using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Puzzle_2023_17_2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var rootFolder = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var input = File.ReadAllText(Path.Combine(rootFolder, "input.txt"));

            var sw = new Stopwatch();
            var strings = input.Trim().Split("\n").Select(s => s.TrimEnd()).ToArray();
            var grid = strings.Select(s => s.Select(c => c - '0').ToArray()).ToArray();
            sw.Start();

            var width = grid[0].Length;
            var height = grid.Length;

            var dists = new int[width, height][];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    dists[x, y] = new int[4 * 10];
                }
            }
            var todo = new Queue<(int x, int y, int dir, int distance, int loss)>();
            todo.Enqueue((0, 0, -1, 0, 0));

            while (todo.Any())
            {
                var next = todo.Dequeue();
                var from = next.loss;

                for (var dir = 0; dir < 4; dir++)
                {
                    if ((dir + 2) % 4 == next.dir || (next.distance < 4 && next.dir != -1 && dir != next.dir))
                    {
                        continue;
                    }
                    var x = next.x;
                    var y = next.y;
                    switch (dir)
                    {
                        case 0: // up
                            y--;
                            break;
                        case 1: // right
                            x++;
                            break;
                        case 2: // down
                            y++;
                            break;
                        case 3: // left
                            x--;
                            break;
                    }
                    if (x < 0 || x >= width || y < 0 || y >= height)
                    {
                        continue;
                    }
                    var to = dists[x, y];
                    var distance = next.dir == dir ? next.distance + 1 : 1;
                    var toIdx = (distance - 1) * 4 + dir;
                    var loss = from + grid[y][x];
                    if (distance > 10 || (to[toIdx] != 0 && loss >= to[toIdx]))
                    {
                        continue;
                    }
                    to[toIdx] = loss;
                    todo.Enqueue((x, y, dir, distance, loss));
                }
            }

            var result = dists[width - 1, height - 1].Where(v => v != 0).Min();

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}