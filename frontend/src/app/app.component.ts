import { Component } from '@angular/core';
import { ScheduleData } from './schedule/one-day-schedule/one-day-schedule.component';
import { HttpClient } from 'selenium-webdriver/http';
import { Http } from '@angular/http';
import { ScheduleService } from './schedule.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  scheduleData: ScheduleData[][][];
  dayIds: number[] = [0, 1, 2, 3, 4, 5, 6];
  weeks: number[] = [0,1,2,3,4];
  testsResult: any;

  constructor(private scheduleService: ScheduleService) {
    
  }

  getSchedule() {
    let res = this.scheduleService.getSolverResponse().subscribe(x => {
      let res = x.json();
      this.scheduleData = res.schedule;
      this.testsResult = JSON.parse(res.testsResult);
    });
    
  }
      
}
