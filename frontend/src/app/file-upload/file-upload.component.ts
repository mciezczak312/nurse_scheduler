import { Component, OnDestroy } from '@angular/core';
import { FileUploadService } from '../common/services/file-upload.service';
import { ToastrService, ActiveToast } from 'ngx-toastr';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.template.html',
  styleUrls: ['./file-upload.styles.css']
})
export class FileUploadComponent implements OnDestroy {

  fileToUpload: File = null;
  private activeToast: ActiveToast<any>;

  constructor(protected fileUploadService: FileUploadService, private toastr: ToastrService) { }

  ngOnDestroy(): void {
    if (this.activeToast) {
      this.toastr.clear(this.activeToast.toastId);
    }
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
    this.fileUploadService.postFile(this.fileToUpload).subscribe(x => {
      this.activeToast = this.toastr.success(x, 'File uploaded successfully');
    });
  }
}
