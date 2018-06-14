using System;

namespace NurseSchedulingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new FirstWeekParser();
            var firstWeek = parser.GetFirstWeekFromFile("first_week_schedule.txt");

            var solver = new Solver(firstWeek);

            while (true)
            {
                int res = solver.Solve();
                if (res == 1 ) break;
            }
            Console.WriteLine(solver.RunTests());
            Console.ReadLine();
        }
    }
}
