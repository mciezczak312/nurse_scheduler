import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { OneDayScheduleComponent } from './schedule/schedule-table/one-day-schedule/one-day-schedule.component';
import { NavbarComponent } from './navbar/navbar/navbar.component';
import { ScheduleService } from './common/services/schedule.service';
import { HttpClientModule } from '@angular/common/http';
import { FakeScheduleService } from './common/services/fake-schedule.service';
import { RouterModule, Routes } from '@angular/router';
import { ScheduleTableComponent} from './schedule/schedule-table/schedule-table.component';
import { ScheduleResolver } from './common/resolvers/schedule.resolver';
import { NurseScheduleComponent } from './schedule/nurse-schedule/nurse-schedule.component';
import { FileUploadComponent } from './file-upload/file-upload.component';
import { FileUploadService } from './common/services/file-upload.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { ConstrainsComponent } from './constrains/constrains.component';
import { NurseScheduleDetailsComponent } from './schedule/nurse-schedule-details/nurse-schedule-details.component';
import { HomeComponent } from './home/home.component';
import { LoadingModule } from 'ngx-loading';
import { SingleDayComponent } from './schedule/nurse-schedule-details/single-day/single-day.component';

const appRoutes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full'
  },
  { path: 'schedule',
    component: ScheduleTableComponent,
    resolve: { schedule : ScheduleResolver }
  },
  {
    path: 'nurses',
    component: NurseScheduleComponent,
    resolve: { schedule : ScheduleResolver }
  },
  {
    path: 'nurses/:id',
    component: NurseScheduleDetailsComponent
  },
  {
    path: 'constraints',
    component: ConstrainsComponent
  }
];


@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes),
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    LoadingModule
  ],
  declarations: [
    AppComponent,
    OneDayScheduleComponent,
    NavbarComponent,
    ScheduleTableComponent,
    NurseScheduleComponent,
    FileUploadComponent,
    ConstrainsComponent,
    NurseScheduleDetailsComponent,
    HomeComponent,
    SingleDayComponent,
  ],
  providers: [
    { provide: ScheduleService, useClass: ScheduleService },
    ScheduleResolver,
    FileUploadService,
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }


