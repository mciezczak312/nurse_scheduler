import { ScheduleData } from "../../schedule/one-day-schedule/one-day-schedule.component";

export class SolverResponse {
    schedule: ScheduleData[][][];
    testsResult: any;
    firstWeek: ScheduleData[][];
}
