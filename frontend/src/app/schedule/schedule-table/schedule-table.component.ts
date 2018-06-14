import { Component, OnDestroy, OnInit } from '@angular/core'
import { ScheduleService } from '../../common/services/schedule.service';
import { ScheduleData } from '../one-day-schedule/one-day-schedule.component';
import { Subscription } from 'rxjs/Subscription';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'pz-schedule-table',
  templateUrl: './schedule-table.template.html',
  styleUrls: ['./schedule-table.styles.css']
})
export class ScheduleTableComponent implements OnDestroy {
  
  scheduleData: ScheduleData[][][];
  dayIds: number[] = [0, 1, 2, 3, 4, 5, 6];
  weeks: number[] = [0,1,2,3,4];
  testsResult: any;

  private subscription: Subscription;

  constructor(
    private scheduleService: ScheduleService,
    private route: ActivatedRoute) {
      
  }

  ngOnInit(): void {
    let data = this.route.snapshot.data['schedule'];
    if (!!data) {
      this.scheduleData = data.schedule;
      this.testsResult = data.testsResult;
    }
  }

  getSchedule() {
    this.subscription = this.scheduleService.getSolverResponse().subscribe(response => {
      this.scheduleData = response.schedule;
      this.testsResult = response.testsResult;
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
