using System.Collections.Generic;

namespace NurseSchedulingApp.API
{
    public class SolverResponse
    {
        public IEnumerable<IEnumerable<IEnumerable<ScheduleDataDTO>>> FirstWeek { get; set; }
        public IEnumerable<IEnumerable<IEnumerable<ScheduleDataDTO>>> Schedule { get; set; }
        public string TestsResult { get; set; }
    }
}
