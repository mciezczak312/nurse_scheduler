import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { tap } from 'rxjs/operators';
import { SolverResponse } from '../models/solver-response';

@Injectable()
export class ScheduleService {

  protected scheduleResponse: SolverResponse;

  constructor(protected http: HttpClient) { }

  getSolverResponse(): Observable<SolverResponse> {
    const res = this.http.get<SolverResponse>('http://localhost:59533/api/schedule')
      .pipe(
        tap(x => this.setScheduleData(x))
      );

    return res;
  }

  protected setScheduleData(x): void {
    this.scheduleResponse = x;
  }

  getScheduleData(): any {
    return this.scheduleResponse;
  }
}


