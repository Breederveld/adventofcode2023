using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2023_12_2
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

            var rows = strings
                .Select(s => s.Split(' '))
                .Select(a => (springs: string.Join('?', a[0], a[0], a[0], a[0], a[0]) + ".", cont: string.Join(',', a[1], a[1], a[1], a[1], a[1]).Split(',').Select(s => int.Parse(s)).ToArray()))
                .ToArray();

            var result = rows.Sum(t =>
            {
                _cache = new Dictionary<(int pos, int contIdx, int count), long>();
                return FindArrangements(t.springs, t.cont);
            });

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

        private static IDictionary<(int pos, int contIdx, int count), long> _cache;
        public static long FindArrangements(string springs, int[] cont, int pos = 0, int contIdx = 0, int count = -1, string collected = null)
        {
            var sum = 0L;
            if (_cache.TryGetValue((pos, contIdx, count), out sum))
            {
                return sum;
            }
            var str = collected ?? string.Empty;

            while (pos < springs.Length)
            {
                var chr = springs[pos];
                switch (chr)
                {
                    case '.':
                        if (count > 0)
                        {
                            return sum;
                        }
                        pos++;
                        count = -1;
                        str += ".";
                        break;
                    case '#':
                        if (count == -1)
                        {
                            if (contIdx == cont.Length)
                            {
                                return sum;
                            }
                            count = cont[contIdx];
                            contIdx++;
                        }
                        count--;
                        pos++;
                        if (count < 0)
                        {
                            return sum;
                        }
                        str += "#";
                        break;
                    case '?':
                        if (count <= 0)
                        {
                            var add = FindArrangements(springs, cont, pos + 1, contIdx, -1, str + ".");
                            _cache[(pos + 1, contIdx, -1)] = add;
                            sum += add;
                        }
                        if (count != 0)
                        {
                            if (count == -1)
                            {
                                if (contIdx == cont.Length)
                                {
                                    return sum;
                                }
                                count = cont[contIdx];
                                contIdx++;
                            }
                            var add = FindArrangements(springs, cont, pos + 1, contIdx, count - 1, str + "#");
                            _cache[(pos + 1, contIdx, count - 1)] = add;
                            sum += add;
                        }
                        return sum;
                }
            }
            if (contIdx == cont.Length && count == -1)
            {
                return 1;
            }
            return 0;
        }
    }
}
