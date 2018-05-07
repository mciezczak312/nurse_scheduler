using System;

namespace NurseSchedulingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new Solver();
            solver.Solve();
            
            
            solver.PrintSolution();
            Console.ReadLine();
        }
    }
}
