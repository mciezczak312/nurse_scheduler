import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SolverResponse } from '../../common/models/solver-response';
import { ScheduleService } from '../../common/services/schedule.service';
import { NurseModel } from '../../common/models/nurse-model';

@Component({
  selector: 'pz-nurse-schedule',
  templateUrl: './nurse-schedule.template.html',
  styleUrls: ['./nurse-schedule.style.css']
})
export class NurseScheduleComponent implements OnInit {

  schedule: SolverResponse = null;
  nurses: NurseModel[];

  constructor(
    private route: ActivatedRoute,
    private scheduleService: ScheduleService
  ) {}

  ngOnInit(): void {
    this.schedule = this.route.snapshot.data['schedule'];
    this.scheduleService.getNursesList().subscribe(x => {
      this.nurses = x;
    });
  }
}
