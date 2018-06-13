using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace NurseSchedulingApp.API
{
    public class SolverResponse
    {
        public IEnumerable<IEnumerable<IEnumerable<ScheduleDataDTO>>> FirstWeek { get; set; }
        public IEnumerable<IEnumerable<IEnumerable<ScheduleDataDTO>>> Schedule { get; set; }
        public JObject TestsResult { get; set; }
    }
}
