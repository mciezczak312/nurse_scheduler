using System;

namespace NurseSchedulingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new Solver();
            while (solver.Solve() == 0)
            {
                
            }
            
            
            solver.PrintSolution();
            Console.ReadLine();
        }
    }
}
