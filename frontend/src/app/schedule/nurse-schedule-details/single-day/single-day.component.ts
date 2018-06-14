import { Input, Component, OnInit } from '@angular/core';

@Component({
  selector: 'pz-single-day',
  templateUrl: './single-day.template.html',
  styleUrls: ['./single-day.styles.css']
})
export class SingleDayComponent implements OnInit {

  @Input()
  shift: string;

  @Input()
  day: number;

  colorClass: string;

  constructor() {}

  ngOnInit(): void {
    this.colorClass = 'shift-' + this.shift.toLowerCase();
  }

  getDayName(day: number): string {
    switch (day) {
      case 0:
        return 'Monday';
      case 1:
        return 'Tuesday';
      case 2:
        return 'Wednesday';
      case 3:
        return 'Thursday';
      case 4:
        return 'Friday';
      case 5:
        return 'Saturday';
      case 6:
        return 'Sunday';
    }
  }

  getShiftHours(shift: string): string {
    switch (shift) {
      case 'EARLY':
        return '7:00 - 16:00';
      case 'DAY':
        return '8:00 - 17:00';
      case 'LATE':
        return '14:00 - 23:00';
      case 'NIGHT':
        return '23:00 - 7:00';
      case 'REST':
        return 'Rest day';
    }
  }
}
