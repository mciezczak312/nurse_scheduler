import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { tap } from 'rxjs/operators';

@Injectable()
export class ScheduleService {

  protected schedule;

  constructor(protected http: HttpClient) { }

  getSolverResponse(): Observable<any> {
    const res = this.http.get('http://localhost:59533/api/schedule')
      .pipe(
        tap(x => this.setSchedule(x))
      );

    return res;
  }

  protected setSchedule(x): void {
    this.schedule = x.schedule;
  }

  getSchedule(): any {
    return this.schedule;
  }
}


