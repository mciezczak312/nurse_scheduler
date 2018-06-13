using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NurseSchedulingApp
{
    enum ShiftsTypes
    {
        Early = 0,
        Day = 1,
        Late = 2,
        Night = 3,
        Rest = 4
    }

    public class Solver
    {
        public List<int> NormalDays { get; set; }
        public List<int> NightShifts { get; set; }
        public List<int> LateShifts { get; set; }
        public List<int> SaturdaysRest { get; set; }
        public List<int> AllShifts { get; set; }
        public int[,] FirstWeek { get; set; }

        public int[] WeekendsDays { get; set; }
        public int[,] Solution { get; set; }
        private const int AllDays = 35;
        private const int AllNurses = 16;
        private Dictionary<int, int> NursesShiftsDict;
        private Random rand = new Random();

        public Solver(int[,] _firstWeek)
        {
            NormalDays = new List<int> { 0, 1, 2, 3, 4, 7, 8, 9, 10, 11, 14, 15, 16, 17, 18, 21, 22, 23, 24, 25, 28, 29, 30, 31, 32 };
            WeekendsDays = new[] { 5, 6, 12, 13, 19, 20, 26, 27, 33, 34 };

            NightShifts = new List<int>();
            for (int i = 0; i < AllDays * 5; i++)
            {
                if (i % 5 == 3) NightShifts.Add(i);
            }
            LateShifts = new List<int>();
            for (int i = 0; i < AllDays * 5; i++)
            {
                if (i % 5 == 2) LateShifts.Add(i);
            }

            AllShifts = Enumerable.Range(0, AllDays * 5).ToList();

            SaturdaysRest = new List<int>();
            for (int i = 0; i < AllDays * 5; i++)
            {
                if (GetDayFromShift(i) % 7 == 5 && GetShiftType(i) == 4)
                {
                    SaturdaysRest.Add(i);
                }
            }

            Solution = new int[16, AllDays * 5];
            FirstWeek = _firstWeek;



            NursesShiftsDict = new Dictionary<int, int>();
            for (int i = 0; i < AllNurses; i++)
            {
                if (i < 12) NursesShiftsDict.Add(i, 23);
                if (i == 12) NursesShiftsDict.Add(i, 20);
                if (i > 12) NursesShiftsDict.Add(i, 13);
            }
        }

        public void AssignNightShifts()
        {
            var randomNumbers = Enumerable.Range(0, AllNurses).OrderBy(x => rand.Next()).ToList();
            int index = 0;

            var reset2 = new List<int>() { 2, 7, 9, 14, 16, 21, 23, 28, 30, 34 };
            var reset3 = new List<int>() { 5, 12, 19, 26, 33 };
            for (int i = 0; i < AllDays; i++)
            {
                if (reset2.Contains(i))
                {
                    if (i < 32)
                    {
                        Solution[randomNumbers[index], (i * 5 + 3) + 1] = 1;
                        Solution[randomNumbers[index], (i * 5 + 3) + 6] = 1;
                    }
                    index++;

                }
                if (reset3.Contains(i))
                {
                    if (i < 32)
                    {
                        Solution[randomNumbers[index], (i * 5 + 3) + 1] = 1;
                        Solution[randomNumbers[index], (i * 5 + 3) + 6] = 1;
                    }
                    index++;
                }

                Solution[randomNumbers[index], i * 5 + 3] = 1;
            }
        }

        public void AssignRestShiftsForWeekends()
        {
            var weekendShifts = new Dictionary<int, Tuple<int, int>>();
            weekendShifts.Add(0, new Tuple<int, int>(29, 34));
            weekendShifts.Add(1, new Tuple<int, int>(29 + 35, 34 + 35));
            weekendShifts.Add(2, new Tuple<int, int>(29 + 35 + 35, 34 + 35 + 35));
            weekendShifts.Add(3, new Tuple<int, int>(29 + 35 + 35 + 35, 34 + 35 + 35 + 35));
            weekendShifts.Add(4, new Tuple<int, int>(29 + 35 + 35 + 35 + 35, 34 + 35 + 35 + 35 + 35));

            foreach (var weekendShift in weekendShifts)
            {
                if (weekendShift.Key == 0)
                {
                    for (int i = 15; i >= 10; i--)
                    {
                        if (i == 10) continue;
                        Solution[i, weekendShift.Value.Item1] = 1;
                        Solution[i, weekendShift.Value.Item2] = 1;
                    }
                }

                if (weekendShift.Key == 1)
                {
                    for (int i = 15; i >= 10; i--)
                    {
                        if (i == 13) continue;
                        Solution[i, weekendShift.Value.Item1] = 1;
                        Solution[i, weekendShift.Value.Item2] = 1;
                    }
                }

                if (weekendShift.Key == 2)
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        Solution[i, weekendShift.Value.Item1] = 1;
                        Solution[i, weekendShift.Value.Item2] = 1;
                    }
                    Solution[0, weekendShift.Value.Item1] = 1;
                    Solution[0, weekendShift.Value.Item2] = 1;
                    Solution[9, weekendShift.Value.Item1] = 1;
                    Solution[9, weekendShift.Value.Item2] = 1;
                }

                if (weekendShift.Key == 3)
                {
                    for (int i = 2; i <= 7; i++)
                    {
                        if (i == 4) continue;
                        Solution[i, weekendShift.Value.Item1] = 1;
                        Solution[i, weekendShift.Value.Item2] = 1;
                    }


                    Solution[8, weekendShift.Value.Item1] = 1;
                    Solution[8, weekendShift.Value.Item2] = 1;
                }

                if (weekendShift.Key == 4)
                {
                    for (int i = 5; i <= 9; i++)
                    {
                        if (i == 7) continue;
                        Solution[i, weekendShift.Value.Item1] = 1;
                        Solution[i, weekendShift.Value.Item2] = 1;
                    }

                    Solution[0, weekendShift.Value.Item1] = 1;
                    Solution[0, weekendShift.Value.Item2] = 1;
                }

            }
        }

        public void AssignFirstWeek()
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 35; j++)
                {
                    Solution[i, j] = FirstWeek[i, j];
                }
            }
        }

        public void RestAfterNights()
        {
            for (int i = 0; i < AllNurses; i++)
            {
                if (FirstWeek[i, 33] == 1)
                {
                    Solution[i, 4] = 1;
                    Solution[i, 9] = 1;
                }
            }
        }

        public int Solve()
        {
            Solution = new int[16, AllDays * 5];
            
            RestAfterNights();
            AssignNightShifts();

            int lastDay = 0;

            for (int shift = 0; shift < AllDays * 5; shift++)
            {

                var day = GetDayFromShift(shift);
                int shiftType = GetShiftType(shift);


                if (lastDay != day)
                {
                    AssignRestShift(lastDay);
                }

                if (shift == 174)
                {
                    AssignRestShift(day);
                }

                if (shiftType == 4 || shiftType == (int)ShiftsTypes.Night)
                    continue;

                int neededNurses = 0;

                if (NormalDays.Contains(day) && shiftType < 3)
                    neededNurses = 3;

                if (WeekendsDays.Contains(day))
                    neededNurses = 2;

                if (shiftType == 3)
                {
                    neededNurses = 1;
                }



                var nursesAlreadyScheduled = 0;

                var randomNumbers = Enumerable.Range(0, AllNurses).OrderBy(x => rand.Next()).ToList();
                var currRandomIndex = 0;
                while (true)
                {
                    int nurseID = randomNumbers[currRandomIndex];
                    currRandomIndex++;
                    if (CheckHardConstrains(nurseID, shift))
                    {
                        Solution[nurseID, shift] = 1;
                        nursesAlreadyScheduled++;

                        if (nursesAlreadyScheduled < neededNurses && currRandomIndex >= randomNumbers.Count)
                        {
                            return 0;
                        }

                        if (nursesAlreadyScheduled == neededNurses)
                        {
                            lastDay = day;

                            break;
                        }
                    }
                    else
                    {
                        if (currRandomIndex >= randomNumbers.Count)
                        {
                            return 0;
                        }
                    }

                }
            }
            return 1;
        }

        private void AssignRestShift(int day)
        {
            for (int nurseId = 0; nurseId < AllNurses; nurseId++)
            {
                int sumOfNoShifts = 0;
                for (int j = day * 5; j < day * 5 + 4; j++)
                {
                    if (Solution[nurseId, j] == 0)
                    {
                        sumOfNoShifts++;
                    }
                }
                if (sumOfNoShifts == 4)
                {
                    Solution[nurseId, day * 5 + 4] = 1;
                }
            }
        }

        private int GetShiftType(int shift)
        {
            return (int)shift % 5;
        }

        private int GetDayFromShift(int shift)
        {
            return (int)Math.Floor(shift / 5.0);
        }

        private bool CheckHardConstrains(int nurseId, int shift)
        {
            //During any period of 24 consecutive hours, at least 11 hours of rest is required.
            if (GetDayFromShift(shift) > 0)
            {
                if (GetShiftType(shift) == (int)ShiftsTypes.Early)
                {
                    if (Solution[nurseId, shift - 3] == 1) return false;
                }
                if (GetShiftType(shift) == (int)ShiftsTypes.Day)
                {
                    if (Solution[nurseId, shift - 4] == 1) return false;
                }
            }

            //one shift per day
            int day = GetDayFromShift(shift);
            for (int j = day * 5; j < day * 5 + 5; j++)
            {
                if (Solution[nurseId, j] == 1)
                {
                    return false;
                }
            }



            //one nurse doesnt want late shifts
            if (nurseId == 0 && GetShiftType(shift) == 2) return false;

            Solution[nurseId, shift] = 1;

            // maximum numbers for each nurse
            for (int i = 0; i < AllNurses; i++)
            {
                int sum = 0;
                for (int j = 0; j < AllDays * 5; j++)
                {
                    if (GetShiftType(j) % 5 != 4)
                    {
                        if (Solution[i, j] == 1) sum++;
                    }
                }
                if (NursesShiftsDict[i] < sum)
                {
                    Solution[nurseId, shift] = 0;
                    return false;
                }
            }
            


            if (day > 6)
            {
                for (int j = 0; j < AllNurses; j++)
                {
                    int left = 0;
                    int right = 6;
                    while (right < day + 1)
                    {
                        int sum = 0;
                        for (int i = 0; i < 7; i++)
                        {
                            if (NurseWorksDay(j, i + left)) sum++;
                        }
                        if (sum > 6)
                        {
                            Solution[nurseId, shift] = 0;
                            return false;
                        }
                        left++;
                        right++;
                    }
                }
            }

            if (day == 0)
            {
                if (GetShiftType(shift) == (int)ShiftsTypes.Early)
                {
                    if (FirstWeek[nurseId, 32] == 1) return false;
                    if (FirstWeek[nurseId, 33] == 1) return false;
                }
                if (GetShiftType(shift) == (int)ShiftsTypes.Day)
                {
                    if (FirstWeek[nurseId, 32] == 1) return false;
                    if (FirstWeek[nurseId, 33] == 1) return false;
                }
                if (GetShiftType(shift) == (int) ShiftsTypes.Late)
                {
                    if (FirstWeek[nurseId, 34] == 1) return false;
                }

            }
            return true;
        }

        bool NurseWorksDay(int nurseID, int day)
        {
            return Solution[nurseID, day * 5] == 1 ||
                   Solution[nurseID, day * 5 + 1] == 1 ||
                   Solution[nurseID, day * 5 + 2] == 1 ||
                   Solution[nurseID, day * 5 + 3] == 1;
        }
        

        public string RunTests()
        {
            var writer = new StreamWriter("schedule.csv");

            for (int i = 0; i < AllNurses; i++)
            {
                writer.Write("Nurse#" + i + ",");
                for (int j = 0; j < AllDays * 5; j++)
                {
                    writer.Write(Solution[i, j] + ",");

                }
                writer.WriteLine();
            }

            var writer2 = new StreamWriter("schedule2.csv");

            for (int i = 0; i < AllNurses; i++)
            {
                writer2.Write("Nurse#" + i + ",");

                for (int j = 0; j < 35; j++)
                {
                    if (FirstWeek[i, j] == 1)
                    {
                        writer2.Write(j % 5 + ",");
                    }
                }

                for (int j = 0; j < AllDays * 5; j++)
                {
                    if (Solution[i, j] == 1)
                    {
                        writer2.Write(j%5+",");
                    }
                }
                writer2.WriteLine();
            }

            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;

            writer.Close();
            writer2.Close();

            System.Diagnostics.Process processCopy = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo =
                new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments =
                        $"/C copy {projectPath}\\NurseSchedulingApp\\schedule.csv {projectPath}\\NurseSchedulingApp\\Resources\\Tests"
                };
            processCopy.StartInfo = startInfo;
            processCopy.Start();

            System.Diagnostics.Process processTests = new System.Diagnostics.Process();

            startInfo =
                new System.Diagnostics.ProcessStartInfo
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                    FileName = "cmd.exe",
                    Arguments =
                        $"/C cd {projectPath}\\NurseSchedulingApp\\Resources\\Tests && py tests.py"
                };
            processTests.StartInfo = startInfo;
            processTests.Start();

            return processTests.StandardOutput.ReadToEnd();


        }
    }
}
