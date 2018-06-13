import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ScheduleService } from './schedule.service';
import { tap } from 'rxjs/operators';

@Injectable()
export class FakeScheduleService extends ScheduleService {

  getSolverResponse(): Observable<any>{
    const res = this.http.get('../../assets/json.json').
    pipe(
      tap(x => this.setSchedule(x))
    );

    return res;
  }
}
