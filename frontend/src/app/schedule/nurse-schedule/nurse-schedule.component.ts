import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SolverResponse } from '../../common/models/solver-response';

@Component({
  selector: 'pz-nurse-schedule',
  templateUrl: './nurse-schedule.template.html',
})
export class NurseScheduleComponent implements OnInit {

  schedule: SolverResponse = null;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.schedule = this.route.snapshot.data['schedule'];
  }
}
