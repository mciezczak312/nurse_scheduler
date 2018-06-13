import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class FakeScheduleService {

  constructor(private http: HttpClient) { }

  getSolverResponse(): Observable<any>{
    return this.http.get('../../assets/json.json');
  }
}
