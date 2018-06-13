using Microsoft.AspNetCore.Mvc;

namespace NurseSchedulingApp.API.Controllers
{
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        [HttpGet]
        public SolverResponse Get()
        {
            var parser = new FirstWeekParser();

            var solver = new Solver(parser.GetFirstWeekFromFile("first_week_schedule.txt", true));
            while (solver.Solve() == 0) ;

            var mapper = new ScheduleDataMapper();
            var dtoSchedule = mapper.MapScheduleToDTO(solver.Solution);
            var dtoFirstWeek = mapper.MapScheduleToDTO(solver.FirstWeek, 35);

            return new SolverResponse
            {
                FirstWeek = dtoFirstWeek,
                Schedule = dtoSchedule,
                TestsResult = solver.RunTests()
            };
        }
    }
}
