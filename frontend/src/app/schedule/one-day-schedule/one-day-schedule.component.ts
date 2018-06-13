import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'pz-one-day-schedule',
  templateUrl: 'one-day-schedule.template.html',
  styleUrls: ['one-day-schedule.style.css']
})
export class OneDayScheduleComponent implements OnInit {

  @Input()
  data: ScheduleData[];

  @Input()
  day: number;

  dayOfWeek: string;

  constructor() {
  }

  ngOnInit() {
    this.dayOfWeek = mapDayOfWeek(this.day);
  }
}

function mapDayOfWeek(day: number) {
  switch (day) {
    case 0: {
      return 'Monday';
    }
    case 1: {
      return 'Tuesday';
    }
    case 2: {
      return 'Wednesday';
    }
    case 3: {
      return 'Thursday';
    }
    case 4: {
      return 'Friday';
    }
    case 5: {
      return 'Saturday';
    }
    case 6: {
      return 'Sunday';
    }
  }
}

export interface ScheduleData {
  nurseId: number,
  nurseName: string,
  shift: 'EARLY' | 'DAY' | 'LATE' | 'NIGHT'
}


