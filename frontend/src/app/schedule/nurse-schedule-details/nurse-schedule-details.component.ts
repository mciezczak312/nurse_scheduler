import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ScheduleService } from '../../common/services/schedule.service';

@Component({
  selector: 'app-nurse-schedule-details',
  templateUrl: './nurse-schedule-details.component.html',
  styleUrls: ['./nurse-schedule-details.component.css']
})
export class NurseScheduleDetailsComponent implements OnInit {

  data : any[];

  constructor(
    private route: ActivatedRoute,
    private scheduleService: ScheduleService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.data = this.scheduleService.getScheduleForNurse(+params.nurseId);
    });
  }

}
