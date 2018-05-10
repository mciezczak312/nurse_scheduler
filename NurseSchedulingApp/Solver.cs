using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace NurseSchedulingApp
{
    class Solver
    {
        public List<int> NormalDays { get; set; }
        public List<int> NightShifts { get; set; }
        public List<int> LateShifts { get; set; }
        public List<int> SaturdaysRest { get; set; }
        public List<int> AllShifts { get; set; }

        public int[] WeekendsDays { get; set; }
        public int[,] Solution { get; set; }
        private const int AllDays = 35;
        private const int AllNurses = 16;
        private Dictionary<int, int> NursesShiftsDict;
        private Random rand = new Random();

        public Solver()
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
            NursesShiftsDict = new Dictionary<int, int>();
            for (int i = 0; i < AllNurses; i++)
            {
                if (i < 12) NursesShiftsDict.Add(i, 23);
                if (i == 12) NursesShiftsDict.Add(i, 20);
                if (i > 12) NursesShiftsDict.Add(i, 13);
            }


        }

        public int Solve()
        {
            Solution = new int[16, AllDays * 5];

            int lastDay = 0;

            for (int shift = 0; shift < AllDays * 5; shift++)
            {
                var day = GetDayFromShift(shift);
                int shiftType = GetShiftType(shift);



                if (lastDay != day)
                {
                    //new day
                    AssignRestShift(lastDay);
                }

                if (shift == 174)
                {
                    AssignRestShift(day);
                }

                if (shiftType == 4)
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

            //maxium number night shifts for nurses is 3
            var nightShiftSum = NightShifts.Count(nightShift => Solution[nurseId, nightShift] == 1);
            if (nightShiftSum > 3)
            {
                Solution[nurseId, shift] = 0;
                return false;
            }



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

            if (day > 3)
            {
                for (int j = 0; j < AllNurses; j++)
                {
                    int left = 0;
                    int right = 2;

                    while (right < day + 1)
                    {

                        var sum = NightShifts.Where(x => x > left * 5 && x <= right * 5 + 5).ToList();


                        if (Solution[j, sum[0]] == 1 && Solution[j, sum[1]] == 1 && Solution[j, sum[2]] == 1)
                        {
                            if (sum[2] + 11 < AllDays * 5)
                            {
                                if (Solution[nurseId, sum[2] + 6] == 0 || Solution[nurseId, sum[2] + 11] == 0)
                                {
                                    Solution[nurseId, shift] = 0;
                                    return false;
                                }

                            }

                        }

                        if (Solution[j, sum[0]] == 1 && Solution[j, sum[1]] == 1)
                        {
                            if (sum[2] + 11 < AllDays * 5)
                            {
                                if (Solution[nurseId, sum[2] + 6] == 0 || Solution[nurseId, sum[2] + 11] == 0)
                                {
                                    Solution[nurseId, shift] = 0;
                                    return false;
                                }
                            }
                        }


                        left++;
                        right++;
                    }

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

        bool WorksNightShift(int nurseID, int day)
        {
            if (day == 0) return Solution[nurseID, 3] == 0;
            return Solution[nurseID, day * 4 - 1] == 0;
        }

        public void PrintSolution()
        {
            Console.WriteLine("Save?");
            Console.ReadKey();
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

            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;

            writer.Close();

            System.Diagnostics.Process processCopy = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo =
                new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments =
                        $"/C copy {projectPath}\\NurseSchedulingApp\\schedule.csv {projectPath}\\NurseSchedulingApp\\Tests"
                };
            processCopy.StartInfo = startInfo;
            processCopy.Start();

            System.Diagnostics.Process processTests = new System.Diagnostics.Process();

            startInfo =
                new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                    FileName = "cmd.exe",
                    Arguments =
                        $"/C cd {projectPath}\\NurseSchedulingApp\\Resources\\Tests && py tests.py"
                };
            processTests.StartInfo = startInfo;
            processTests.Start();

            Console.WriteLine("Done");
        }
    }
}
