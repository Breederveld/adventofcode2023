using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day04
    {
        string input;

        public Day04()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var numbers = new List<int>();
            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.Inputs.Day04.txt"))
            using (var reader = new StreamReader(stream))
            {
                input = reader.ReadToEnd();
            }
        }

        public string SolveFirstPuzzle()
        {
            var lines = input.Split("\r\n");
            var ticketsSum = lines.Select(line =>
            {
                var parts = line.Split(new char[] { ':', '|' });
                var r = new
                {
                    Ticket = parts[1].Trim().Split(" ").Where(s => s != string.Empty).Select(n => int.Parse(n.Trim())).ToList(),
                    Numbers = parts[2].Trim().Split(" ").Where(s => s != string.Empty).Select(n => int.Parse(n.Trim())).ToList(),
                };
                return r;
            }).Select(t => {
                var lucky = t.Ticket.Intersect(t.Numbers);
                return lucky.Any() ? Math.Pow(2, lucky.Count() - 1) : 0;
            }
            ).Sum();
            return ticketsSum.ToString();
        }

        public string SolveSecondPuzzle()
        {
            var lines = input.Split("\r\n");
            var allCards = Enumerable.Range(1, lines.Length).ToDictionary(k => k, v => 1);
            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(new char[] { ':', '|' });
                var Ticket = parts[1].Trim().Split(" ").Where(s => s != string.Empty).Select(n => int.Parse(n.Trim())).ToList();
                var Numbers = parts[2].Trim().Split(" ").Where(s => s != string.Empty).Select(n => int.Parse(n.Trim())).ToList();
                var lucky = Ticket.Intersect(Numbers).Count();
                if(lucky > 0)
                {
                    Enumerable.Range(i + 2, lucky).ToList().ForEach(index => {
                        if (allCards.ContainsKey(index))
                        {
                            allCards[index] += allCards[i+1];
                        }
                    });
                }
            };
            return allCards.Values.Sum().ToString();
        }
    }
}
