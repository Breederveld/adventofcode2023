using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2023_16_1
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

            var width = strings[0].Length;
            var height = strings.Length;
            var beams = new Queue<(int x, int y, int dir)>();
            // dir: up, right, down, left
            beams.Enqueue((0, 0, 1));

            var energised = new int[width, height];
            while (beams.Any())
            {
                var curr = beams.Dequeue();
                (var x, var y, var dir) = curr;
                while (x >= 0 && x < width && y >= 0 && y < height)
                {
                    if ((energised[x, y] & 1 << dir) != 0)
                    {
                        break;
                    }
                    energised[x, y] |= 1 << dir;
                    switch (strings[y][x])
                    {
                        case '/':
                            dir = dir == 0 ? 1 : dir == 1 ? 0 : dir == 2 ? 3 : 2;
                            break;
                        case '\\':
                            dir = dir == 0 ? 3 : dir == 1 ? 2 : dir == 2 ? 1 : 0;
                            break;
                        case '|':
                            if (dir == 1 || dir == 3)
                            {
                                dir = 0;
                                beams.Enqueue((x, y, 2));
                            }
                            break;
                        case '-':
                            if (dir == 0 || dir == 2)
                            {
                                dir = 1;
                                beams.Enqueue((x, y, 3));
                            }
                            break;
                    }
                    switch (dir)
                    {
                        case 0:
                            y--;
                            break;
                        case 1:
                            x++;
                            break;
                        case 2:
                            y++;
                            break;
                        case 3:
                            x--;
                            break;
                    }
                }
            }

            var result = Enumerable.Range(0, width).Sum(x => Enumerable.Range(0, height).Count(y => energised[x, y] != 0));

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}