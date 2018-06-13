using System;
using System.IO;

namespace NurseSchedulingApp
{
    class FirstWeekParser
    {
        public int[,] GetFirstWeekFromFile(string fileName)
        {
            var solution = new int[16, 35];
            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var filePath = Path.Combine(projectPath, $"NurseSchedulingApp\\Resources\\{fileName}");

            using (var freader = new StreamReader(filePath))
            {
                int nurseId = 0;
                while (!freader.EndOfStream)
                {
                    var shifts = freader.ReadLine()?.Split(",");

                    var day = 0;
                    foreach (var shift in shifts)
                    {
                        var shiftInt = int.Parse(shift);
                        solution[nurseId, (5*day) + shiftInt - 1] = 1;
                        day++;
                    }
                    nurseId++;
                }
            }
            return solution;
        }
    }
}
