using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2023_03_1
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
            var size = strings.Length * width;
            var result = 0;
            var isNum = new Func<char, bool>(chr => chr >= '0' && chr <= '9');
            var isPart = new Func<char, bool>(chr => !isNum(chr) && chr != '.');
            for (int i = 0; i < size; i++)
            {
                var x = i % width;
                var y = i / width;
                var chr = strings[y][x];
                var hasPart = false;
                if (isNum(chr))
                {
                    if (x > 0)
                    {
                        if (isPart(strings[y][x - 1]))
                            hasPart = true;
                        if (y > 0 && isPart(strings[y - 1][x - 1]))
                            hasPart = true;
                        if (y < width - 1 && isPart(strings[y + 1][x - 1]))
                            hasPart = true;
                    }
                    var num = 0;
                    for (var xx = x; xx <= width; xx++)
                    {
                        chr = xx == width ? '.' : strings[y][xx];
                        if (isNum(chr))
                        {
                            num = num * 10 + chr - '0';
                            if (y > 0 && isPart(strings[y - 1][xx]))
                            {
                                hasPart = true;
                            }
                            if (y < height - 1 && isPart(strings[y + 1][xx]))
                            {
                                hasPart = true;
                            }
                        }
                        else
                        {
                            if (xx < width - 1)
                            {
                                if (isPart(strings[y][xx]))
                                    hasPart = true;
                                if (y > 0 && isPart(strings[y - 1][xx]))
                                    hasPart = true;
                                if (y < width - 1 && isPart(strings[y + 1][xx]))
                                    hasPart = true;
                            }
                            if (hasPart)
                            {
                                result += num;
                            }
                            i = y * width + xx - 1;
                            break;
                        }
                    }
                }
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}
