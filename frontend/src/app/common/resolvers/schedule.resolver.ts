import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { ScheduleService } from '../services/schedule.service';

@Injectable()
export class ScheduleResolver implements Resolve<any> {
  constructor(private scheduleService: ScheduleService) {}

  resolve() {
    const schedule = this.scheduleService.getScheduleData();
    if (schedule) {
      return JSON.parse(JSON.stringify(this.scheduleService.getScheduleData()));
    } else {
      return null;
    }
  }
}
