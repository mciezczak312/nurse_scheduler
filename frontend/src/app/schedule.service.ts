import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class ScheduleService {

  constructor(private http: Http) { }

  getSolverResponse(): Observable<any>{
    return this.http.get('http://localhost:59533/api/schedule');
  }

}
