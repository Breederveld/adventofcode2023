using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeTech.Core.Mathematics.Cycles;
using MoreLinq;

namespace Puzzle_2023_14_2
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
            var grid = new int[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var chr = strings[y][x];
                    grid[x, y] = chr == 'O' ? 1 : chr == '#' ? 2 : 0;
                }
            }

            var cycleDetector = new StreamCycleDetector<int>() { MinRepeats = 10 };
            var result = cycleDetector.PredictAt(Run(grid), 1000000000 - 1);

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

        private static IEnumerable<int> Run(int[,] grid)
        {
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);
            for (int cycle = 0; cycle < 10000; cycle++)
            {
                // N
                for (var x = 0; x < width; x++)
                {
                    var next = 0;
                    var cnt = 0;
                    for (var y = next; y < height; y++)
                    {
                        if (grid[x, y] == 2)
                        {
                            for (var yy = 0; yy < cnt; yy++)
                            {
                                grid[x, next + yy] = 1;
                            }
                            next = y + 1;
                            cnt = 0;
                        }
                        if (grid[x, y] == 1)
                        {
                            cnt++;
                            grid[x, y] = 0;
                        }
                    }
                    for (var yy = 0; yy < cnt; yy++)
                    {
                        grid[x, next + yy] = 1;
                    }
                }
                // W
                for (var y = 0; y < height; y++)
                {
                    var next = 0;
                    var cnt = 0;
                    for (var x = next; x < width; x++)
                    {
                        if (grid[x, y] == 2)
                        {
                            for (var xx = 0; xx < cnt; xx++)
                            {
                                grid[next + xx, y] = 1;
                            }
                            next = x + 1;
                            cnt = 0;
                        }
                        if (grid[x, y] == 1)
                        {
                            cnt++;
                            grid[x, y] = 0;
                        }
                    }
                    for (var xx = 0; xx < cnt; xx++)
                    {
                        grid[next + xx, y] = 1;
                    }
                }
                // S
                for (var x = 0; x < width; x++)
                {
                    var next = height - 1;
                    var cnt = 0;
                    for (var y = next; y >= 0; y--)
                    {
                        if (grid[x, y] == 2)
                        {
                            for (var yy = 0; yy < cnt; yy++)
                            {
                                grid[x, next - yy] = 1;
                            }
                            next = y - 1;
                            cnt = 0;
                        }
                        if (grid[x, y] == 1)
                        {
                            cnt++;
                            grid[x, y] = 0;
                        }
                    }
                    for (var yy = 0; yy < cnt; yy++)
                    {
                        grid[x, next - yy] = 1;
                    }
                }
                // E
                for (var y = 0; y < height; y++)
                {
                    var next = width - 1;
                    var cnt = 0;
                    for (var x = next; x >= 0; x--)
                    {
                        if (grid[x, y] == 2)
                        {
                            for (var xx = 0; xx < cnt; xx++)
                            {
                                grid[next - xx, y] = 1;
                            }
                            next = x - 1;
                            cnt = 0;
                        }
                        if (grid[x, y] == 1)
                        {
                            cnt++;
                            grid[x, y] = 0;
                        }
                    }
                    for (var xx = 0; xx < cnt; xx++)
                    {
                        grid[next - xx, y] = 1;
                    }
                }
                var sum = Enumerable.Range(0, height).Sum(y => Enumerable.Range(0, width).Count(x => grid[x, y] == 1) * (height - y));
                yield return sum;
            }
        }
    }
}