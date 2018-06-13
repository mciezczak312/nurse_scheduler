using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NurseSchedulingApp.API
{
    public class SolverResponse
    {
        public IEnumerable<IEnumerable<IEnumerable<ScheduleDataDTO>>> Schedule { get; set; }
        public string TestsResult { get; set; }
    }
}
