import { ScheduleData } from "../../schedule/schedule-table/one-day-schedule/one-day-schedule.component";

export class SolverResponse {
    schedule: ScheduleData[][][];
    hardConstraintsTestsResult: any;
    softConstraintsTestsResult: any;
    firstWeek: ScheduleData[][];
}
