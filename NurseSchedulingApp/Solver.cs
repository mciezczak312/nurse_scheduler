using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NurseSchedulingApp
{
    class Solver
    {
        public List<int> NormalDays { get; set; }
        public List<int> NightShifts { get; set; }
        public List<int> SaturdaysRest { get; set; }

        public int[] WeekendsDays { get; set; }
        public int[,] Solution { get; set; }
        private const int AllDays = 35;
        private const int AllNurses = 16;
        private Dictionary<int, int> NursesShiftsDict;
        private Random rand = new Random();

        public Solver()
        {
            NormalDays = new List<int>{ 0, 1, 2, 3, 4, 7, 8, 9, 10, 11, 14, 15, 16, 17, 18, 21, 22, 23, 24, 25, 28, 29, 30, 31, 32 };
            WeekendsDays = new[] { 5, 6, 12, 13, 19, 20, 26, 27, 33, 34 };
            NightShifts = new List<int>();
            for (int i = 0; i < AllDays * 5; i++)
            {
                if(i%5 == 4) NightShifts.Add(i);
            }

            SaturdaysRest = new List<int>();
            for (int i = 0; i < AllDays*5; i++)
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

        public void Solve()
        {
            int nursesAlreadyScheduled = 0;

            for (int shift = 0; shift < AllDays * 5; shift++)
            {
                int day = GetDayFromShift(shift);
                int shiftType = GetShiftType(shift);
                int neededNurses = 0;

                if (NormalDays.Contains(day) && shiftType < 3)
                    neededNurses = 3;

                if (!NormalDays.Contains(day))
                    neededNurses = 2;

                if (shiftType == 3 || shiftType == 4)
                {
                    neededNurses = 1;
                }
                
                while (true)
                {
                    int nurseID = rand.Next(0, AllNurses);
                    if (CheckHardConstrains(nurseID, shift))
                    {
                        Solution[nurseID, shift] = 1;
                        nursesAlreadyScheduled++;
                        if (nursesAlreadyScheduled == neededNurses)
                        {
                            nursesAlreadyScheduled = 0;
                            break;
                        }
                            
                    }
                }
            }
        }

        private int GetShiftType(int shift)
        {
            return (int) shift % 5;
        }

        private int GetDayFromShift(int shift)
        {
            return (int)Math.Floor(shift / 5.0);
        }

        private bool CheckHardConstrains(int nurseId, int shift)
        {
            //one shift per day
            int day = GetDayFromShift(shift);
            for (int j = day; j < day + 5; j++)
            {
                if (Solution[nurseId, j] == 1)
                {
                    return false;
                }
            }

            //maxium number night shifts for nurses is 3
            Solution[nurseId, shift] = 1;
            for (int i = 0; i < AllNurses; i++)
            {
                int sum = 0;
                for (int j = 0; j < AllDays * 5; j++)
                {
                    if (GetShiftType(j) == 4 && Solution[i, j] == 1)
                    {
                        sum++;
                    }
                    if (sum > 3)
                    {
                        Solution[nurseId, shift] = 0;
                        return false;
                    }
                }
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

            // 2 weekends off
            for (int i = 0; i < AllNurses; i++)
            {
                int sum = 0;
                foreach (var rest in SaturdaysRest)
                {
                    if (Solution[i, rest] == 0 && Solution[i, rest + 5] == 0) sum++;
                }
                if (sum < 2)
                {
                    Solution[nurseId, shift] = 0;
                    return false;
                }
            }



            return true;
        }

        public void PrintSolution()
        {
            var writer = new StreamWriter("schedule.csv");

            for (int i = -1; i < AllDays * 5; i++)
            {
                writer.Write(i + ",");
            }
            writer.WriteLine();


            for (int i = 0; i < AllNurses; i++)
            {
                writer.Write("Nurse#" + i + ",");
                for (int j = 0; j < AllDays * 5; j++)
                {

                    writer.Write(Solution[i,j] + ",");

                }
                writer.WriteLine();
            }
            writer.Close();
            Console.WriteLine("Done");
        }
    }
}
