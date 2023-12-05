using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day02
    {
        string input;

        public Day02()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var numbers = new List<int>();
            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.Inputs.Day02.txt"))
            using (var reader = new StreamReader(stream))
            {
                input = reader.ReadToEnd();
            }
        }

        public string SolveFirstPuzzle()
        {
            var config = new Dictionary<string, int> { { "red", 12 }, { "green", 13 }, { "blue", 14 } };
            var lines = input.Split("\r\n");
            var possibleGamesSum = 0;
            foreach (var line in lines)
            {
                var mainParts = line.Split(": ");
                var gameId = int.Parse(mainParts[0].Split(" ")[1]);
                var cubes = mainParts[1].Split(new char[] {';', ','});
                if (cubes.Any(c =>
                {
                    var cubeStringParts = c.Trim().Split(" ");
                    var numberOfCubes = int.Parse(cubeStringParts[0]);
                    var cubeColor = cubeStringParts[1];
                    return config[cubeColor] < numberOfCubes;
                })) continue;
                possibleGamesSum += gameId;
            }
            return possibleGamesSum.ToString();
        }

        public string SolveSecondPuzzle()
        {
            var lines = input.Split("\r\n");
            int totalGamesPower = 0;
            foreach (var line in lines)
            {
                var mainParts = line.Split(": ");
                var cubes = mainParts[1].Split(new char[] { ';', ',' });
                var config = new Dictionary<string, int> { { "red", 1 }, { "green", 1 }, { "blue", 1 } };
                foreach (var cube in cubes)
                {
                    var cubeStringParts = cube.Trim().Split(" ");
                    var numberOfCubes = int.Parse(cubeStringParts[0]);
                    var cubeColor = cubeStringParts[1];
                    if (config[cubeColor] < numberOfCubes)
                    {
                        config[cubeColor] = numberOfCubes;
                    }
                }
                totalGamesPower += (config["red"] * config["green"] * config["blue"]);
            }
            return totalGamesPower.ToString();
        }
    }
}
