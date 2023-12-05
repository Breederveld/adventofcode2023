using System.Reflection;

namespace AdventOfCode2023
{
    internal class Day03
    {
        string input;

        public Day03()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var numbers = new List<int>();
            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.Inputs.Day03.txt"))
            using (var reader = new StreamReader(stream))
            {
                input = reader.ReadToEnd();
            }
        }

        public string SolveFirstPuzzle()
        {
            var map = input.Split("\r\n");
            var sum = 0;
            for(int row = 0; row < map.Length; row++)
            {
                var curentNumber = "";
                for(int col = 0; col < map[row].Length; col++)
                {
                    if (map[row][col] >= '0' && map[row][col] <= '9')
                    {
                        curentNumber += map[row][col];
                        if (col < map[row].Length - 1)
                        {
                            continue;
                        }
                    }
                    if (curentNumber.Length > 0) //completed the number
                    {
                        var number = int.Parse(curentNumber);
                        var currentNumberLength = curentNumber.Length;
                        curentNumber = "";
                        var leftCol = col-currentNumberLength-1;
                        if (leftCol < 0)
                        {
                            leftCol = 0;
                        }
                        var rightCol = col;
                        // check above row
                        if (row >= 1)
                        {
                            var toCheckAbove = map[row - 1].Substring(leftCol, rightCol-leftCol+1);
                            if (ContainsSymbol(toCheckAbove))
                            {
                                sum += number;
                                continue;
                            }
                        }
                        //check current row
                        if (ContainsSymbol(map[row].Substring(leftCol, rightCol - leftCol+1)))
                        {
                            sum += number;
                            continue;
                        }
                        //check below row
                        if (row < map.Length - 1)
                        {
                            var toCheckBelow = map[row + 1].Substring(leftCol, rightCol-leftCol+1);
                            if (ContainsSymbol(toCheckBelow))
                            {
                                sum += number;
                                continue;
                            }
                        }
                    }
                }
            }
            return sum.ToString();
        }

        public string SolveSecondPuzzle()
        {
            var map = input.Split("\r\n");
            var sum = 0;
            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    if (map[row][col] == '*')
                    {
                        var adjacentNumbers = new List<int>();
                        var leftCol = col - 1 < 0 ? 0 : col - 1;
                        var rightCol = col + 1 == map[row].Length ? col : col + 1;
                        //Check from above
                        if (row > 0)
                        {
                            GetAdjacentNumberForRow(map, row-1, adjacentNumbers, leftCol);
                        }
                        //Check left of the star
                        if (char.IsDigit(map[row][leftCol]))
                        {
                            var leftCheck = GetNumberLeftOfPosition(map[row], leftCol);
                            adjacentNumbers.Add(int.Parse(leftCheck.number));
                        }
                        //Check right of the star
                        if (char.IsDigit(map[row][rightCol]))
                        {
                            var rightCheck = GetNumberRightOfPosition(map[row], rightCol);
                            adjacentNumbers.Add(int.Parse(rightCheck.number));
                        }
                        //Check row bellow
                        if (row < map.Length - 1)
                        {
                            GetAdjacentNumberForRow(map, row+1, adjacentNumbers, leftCol);
                        }
                        if (adjacentNumbers.Count == 2)
                        {
                            sum += adjacentNumbers[0] * adjacentNumbers[1];
                        }
                    }
                }
            }
            return sum.ToString();
        }

        private void GetAdjacentNumberForRow(string[] map, int row, List<int> adjacentNumbers, int leftCol)
        {
            if (char.IsDigit(map[row][leftCol]))
            {
                var leftCheck = this.GetNumberLeftOfPosition(map[row], leftCol);
                adjacentNumbers.Add(int.Parse(leftCheck.number));
                var currentPosition = leftCheck.nextPositionToCheck;
                while (currentPosition <= leftCol + 2 && currentPosition < map[row].Length)
                {
                    if (char.IsDigit(map[row][currentPosition]))
                    {
                        var rightCheck = GetNumberRightOfPosition(map[row], currentPosition);
                        currentPosition = rightCheck.nextPositionToCheck;
                        adjacentNumbers.Add(int.Parse(rightCheck.number));
                    }
                    else
                    {
                        currentPosition++;
                    }
                }
            }
            else
            {
                var currentPosition = leftCol + 1;
                while (currentPosition <= leftCol + 2 && currentPosition < map[row].Length)
                {
                    if (char.IsDigit(map[row][currentPosition]))
                    {
                        var rightCheck = GetNumberRightOfPosition(map[row], currentPosition);
                        currentPosition = rightCheck.nextPositionToCheck;
                        adjacentNumbers.Add(int.Parse(rightCheck.number));
                    }
                    else
                    {
                        currentPosition++;
                    }
                }
            }
        }

        private (string number, int nextPositionToCheck) GetNumberLeftOfPosition(string row, int position)
        {
            var currentNumber = "";
            currentNumber += row[position];
            var currentPosition = position - 1;
            while (currentPosition >= 0 && char.IsDigit(row[currentPosition]))
            {
                currentNumber = row[currentPosition] + currentNumber;
                currentPosition--;
            }
            currentPosition = position + 1;
            while (currentPosition < row.Length && char.IsDigit(row[currentPosition]))
            {
                currentNumber += row[currentPosition];
                currentPosition++;
            }
            return (currentNumber, currentPosition);
        }

        private (string number, int nextPositionToCheck) GetNumberRightOfPosition(string row, int position)
        {
            var currentNumber = row[position].ToString();
            position++;
            while(position<row.Length && char.IsDigit(row[position]))
            {
                currentNumber += row[position];
                position++;
            }
            return (currentNumber, position);
        }

        private bool ContainsSymbol(string subString)
        {
            return subString.Any(c => !Char.IsDigit(c) && c != '.');
        }
    }
}
