using System;

namespace NurseSchedulingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var parser = new FirstWeekParser();
            //parser.GetFirstWeekFromFile("first_week_schedule.txt");

            var solver = new Solver();
            while (solver.Solve() == 0)
            {

            }


            solver.RunTests();
            Console.ReadLine();
        }
    }
}
