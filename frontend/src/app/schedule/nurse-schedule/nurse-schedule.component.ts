import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SolverResponse } from '../../common/models/solver-response';
import { ScheduleService } from '../../common/services/schedule.service';
import { NurseModel } from '../../common/models/nurse-model';
import { ToastrService, ActiveToast } from 'ngx-toastr';

@Component({
  selector: 'pz-nurse-schedule',
  templateUrl: './nurse-schedule.template.html',
  styleUrls: ['./nurse-schedule.style.css']
})
export class NurseScheduleComponent implements OnInit, OnDestroy {

  schedule: SolverResponse = null;
  nurses: NurseModel[];
  private activeToast: ActiveToast<any>;

  constructor(
      private router: Router,
      private route: ActivatedRoute,
      private toastr: ToastrService,
      private scheduleService: ScheduleService) {}

  ngOnInit(): void {
    this.schedule = this.route.snapshot.data['schedule'];
    this.scheduleService.getNursesList().subscribe(nurses => {
      this.nurses = nurses;
      if (!this.schedule) {
        this.activeToast = this.toastr.warning(
          'Go to "Schedule table" page to generate a new schedule',
          'There is no schedule generated',
        );
      }
    });
  }

  navigateToDetails(id: number) {
      this.router.navigate([`/nurses/${id}`])
  }

  ngOnDestroy(): void {
    if (this.activeToast) {
      this.toastr.clear(this.activeToast.toastId);
    }
  }
}
