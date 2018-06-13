import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class ScheduleService {

  constructor(private http: HttpClient) { }

  getSolverResponse(): Observable<any>{
    return this.http.get('http://localhost:59533/api/schedule');
  }

}
