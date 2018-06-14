import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core'
import { ScheduleService } from '../../common/services/schedule.service';
import { ScheduleData } from '../one-day-schedule/one-day-schedule.component';
import { Subscription } from 'rxjs/Subscription';
import { ActivatedRoute } from '@angular/router';
import { SolverResponse } from '../../common/models/solver-response';

@Component({
  selector: 'pz-schedule-table',
  templateUrl: './schedule-table.template.html',
  styleUrls: ['./schedule-table.styles.css']
})
export class ScheduleTableComponent implements OnDestroy {
  
  scheduleModel: SolverResponse;

  scheduleCost: number;
  dayIds: number[] = [0, 1, 2, 3, 4, 5, 6];
  weeks: number[] = [0,1,2,3,4];

  private subscription: Subscription;

  constructor(
    private scheduleService: ScheduleService,
    private route: ActivatedRoute) {
      
  }

  ngOnInit(): void {
    let data = this.route.snapshot.data['schedule'];
    this.scheduleModel = {...data};
    this.scheduleCost = this.calculateCost();
    
  }

  getSchedule() {
    this.subscription = this.scheduleService.getSolverResponse().subscribe(response => {
      this.scheduleModel = {...response}
      this.scheduleCost = this.calculateCost();
    });
  }

  calculateCost() {
    let sum = 0;
    Object.keys(this.scheduleModel.softConstraintsTestsResult).forEach(x => 
      sum = sum + this.scheduleModel.softConstraintsTestsResult[x]
    );
    return sum;
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
