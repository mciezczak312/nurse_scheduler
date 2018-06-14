import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs/observable/of';

@Injectable()
export class FileUploadService {

  constructor(protected http: HttpClient){ }

  postFile(fileToUpload: File): Observable<any> {
    const endpoint = 'http://localhost:59533/api/schedule/uploadFile';
    const formData: FormData = new FormData();
    formData.append('fileKey', fileToUpload, fileToUpload.name);
    return this.http
      .post(endpoint, formData);
}
}
