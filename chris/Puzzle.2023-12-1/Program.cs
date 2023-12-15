using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace Puzzle_2023_12_1
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
                .Select(a => (springs: a[0], cont: a[1].Split(',').Select(s => int.Parse(s)).ToArray()))
                .ToArray();


            var result = rows.Sum(t => FindArrangements(t.springs, t.cont));

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

        private static int FindArrangements(string springs, int[] cont)
        {
            var qs = springs.FindIndexes(c => c == '?').ToArray();
            var option = new bool[qs.Length];
            var d = 0;
            var sum = 0;
            if (IsValid(springs, cont, option))
            {
                sum++;
            }
            while (d < option.Length)
            {
                while (d < option.Length)
                {
                    if (option[d])
                    {
                        option[d] = false;
                        d++;
                    }
                    else
                    {
                        option[d] = true;
                        d = 0;
                        break;
                    }
                }
                if (d == option.Length)
                    break;
                if (IsValid(springs, cont, option))
                {
                    sum++;
                }
            }
            return sum;
        }

        private static bool IsValid(string springs, int[] cont, bool[] option)
        {
            var contPos = 0;
            var optionPos = 0;
            var cnt = 0;
            for (var i = 0; i < springs.Length; i++)
            {
                var s = springs[i];
                if (s == '?')
                {
                    s = option[optionPos] ? '#' : '.';
                    optionPos++;
                }
                if (s == '#')
                {
                    cnt++;
                }
                else
                {
                    if (cnt > 0)
                    {
                        if (contPos < cont.Length && cont[contPos] != cnt)
                        {
                            return false;
                        }
                        contPos++;
                        cnt = 0;
                    }
                }
            }
            if (cnt > 0)
            {
                if (contPos < cont.Length && cont[contPos] != cnt)
                {
                    return false;
                }
                contPos++;
            }
            return contPos == cont.Length;
        }
    }
}