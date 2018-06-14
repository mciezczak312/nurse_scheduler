import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable()
export class FileUploadService {

  constructor(protected http: HttpClient){ }

  postFile(fileToUpload: File): Observable<any> {
    const endpoint = environment.apiUrl+'api/schedule/uploadFile';
    const formData: FormData = new FormData();
    formData.append('fileKey', fileToUpload, fileToUpload.name);
    return this.http
      .post(endpoint, formData);
  }
}
