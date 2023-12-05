using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day01
    {
        string input;

        public Day01()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var numbers = new List<int>();
            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.Inputs.Day01a.txt"))
            using (var reader = new StreamReader(stream))
            {
                input = reader.ReadToEnd();

            }
        }

        public string SolveFirstPuzzle()
        {
            var numbers = input.Split("\n").Select(line => int.Parse($"{line.First(c => c >= '0' && c <= '9')}{line.Last(c => c >= '0' && c <= '9')}")).ToList();
            return numbers.Sum().ToString();
        }

        public string SolveSecondPuzzle()
        {
            var spelledNumbers = new List<string> { "one", "1", "two", "2", "three", "3", "four", "4", "five", "5", "six", "6", "seven", "7", "eight", "8", "nine","9"};
            var numbers = new List<int>();
            foreach (var line in input.Split("\r\n"))
            {
                var orderedSmallIndexes = spelledNumbers.ToDictionary(k => k, k => line.IndexOf(k)).Where(kvp => kvp.Value > -1).OrderBy(kvp=>kvp.Value);
                var orderedHighIndexes = spelledNumbers.ToDictionary(k => k, k => line.LastIndexOf(k)).Where(kvp => kvp.Value > -1).OrderBy(kvp => kvp.Value);
                var firstDigit = spelledNumbers.IndexOf(orderedSmallIndexes.First().Key) / 2 + 1;
                var lasDigit = spelledNumbers.IndexOf(orderedHighIndexes.Last().Key) / 2 + 1;
                var theNumber = int.Parse($"{firstDigit}{lasDigit}");
                numbers.Add(theNumber);
            }
            return numbers.Sum().ToString();
        }
    }
}
