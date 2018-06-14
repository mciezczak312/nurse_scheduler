import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { tap } from 'rxjs/operators';
import { SolverResponse } from '../models/solver-response';
import { NurseModel } from '../models/nurse-model';
import { environment } from '../../../environments/environment';

@Injectable()
export class ScheduleService {

  protected scheduleResponse: SolverResponse;
  private apiUrl = environment.apiUrl

  constructor(protected http: HttpClient) { }

  getSolverResponse(): Observable<SolverResponse> {
    
    const res = this.http.get<SolverResponse>(this.apiUrl+'api/schedule')
      .pipe(
        tap(x => this.setScheduleData(x))
      );

    return res;
  }

  getNursesList(): Observable<NurseModel[]> {
    return this.http.get<NurseModel[]>(this.apiUrl+'api/schedule/nursesList')
  }

  getScheduleForNurse(id: number):any[] {
    const res = [];
    this.scheduleResponse.schedule.forEach((x, indexWeek) => {
      x.forEach((z, indexDay) => {
        z.forEach(obj => {
          if (obj.nurseId === id) {
            res.push({...obj, week: indexWeek, day: indexDay});
          }
        });
      });
    });
    return res;
  }

  protected setScheduleData(x): void {
    this.scheduleResponse = x;
  }

  getScheduleData(): any {
    return this.scheduleResponse;
  }
}


