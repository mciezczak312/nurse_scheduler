namespace NurseSchedulingApp.API
{
    public enum ShiftType
    {
        EARLY, DAY, LATE, NIGHT
    }


    public class ScheduleDataDTO
    {
        public int NurseId { get; set; }
        public string NurseName { get; set; }
        public string Shift { get; set; }
    }
}