import { Component, OnInit } from '@angular/core';
import { FileUploadService } from '../common/services/file-upload.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {

  fileToUpload: File = null;

  constructor(
    protected fileUploadService: FileUploadService,
    private toastr: ToastrService) { }

  ngOnInit() {
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
    this.fileUploadService.postFile(this.fileToUpload).subscribe(x => {
      this.toastr.success(x, 'File uploaded successfully');
    });
  }
}
