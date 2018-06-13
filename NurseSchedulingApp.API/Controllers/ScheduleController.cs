using Microsoft.AspNetCore.Mvc;

namespace NurseSchedulingApp.API.Controllers
{
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        [HttpGet]
        public SolverResponse Get()
        {
            var solver = new Solver();
            while (solver.Solve() == 0) ;

            var mapper = new ScheduleDataMapper();
            var dto = mapper.MapScheduleToDTO(solver.Solution);

            return new SolverResponse
            {
                Schedule = dto,
                TestsResult = solver.RunTests()
            };
        }
    }
}
