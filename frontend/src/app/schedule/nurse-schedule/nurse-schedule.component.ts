import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'pz-nurse-schedule',
  templateUrl: './nurse-schedule.template.html',
})
export class NurseScheduleComponent implements OnInit {

  schedule: any = null;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.schedule = this.route.snapshot.data['schedule'];
    console.log(this.schedule);
  }
}
