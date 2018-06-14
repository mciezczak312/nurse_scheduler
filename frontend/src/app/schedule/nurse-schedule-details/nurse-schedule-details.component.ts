import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ScheduleService } from '../../common/services/schedule.service';
import { NurseModel } from '../../common/models/nurse-model';

@Component({
  selector: 'app-nurse-schedule-details',
  templateUrl: './nurse-schedule-details.template.html',
  styleUrls: ['./nurse-schedule-details.styles.css']
})
export class NurseScheduleDetailsComponent implements OnInit {

  data : any[];
  nurse: NurseModel;

  constructor(
    private route: ActivatedRoute,
    private scheduleService: ScheduleService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.data = this.scheduleService.getScheduleForNurse(+params.nurseId);
      console.log(this.data);
      this.scheduleService.getNursesList().subscribe(nurses => {
        this.nurse = nurses.filter(nurse => nurse.id == params.nurseId)[0];
      });
    });
  }
}
