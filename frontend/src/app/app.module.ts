import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { OneDayScheduleComponent } from './schedule/one-day-schedule/one-day-schedule.component';
import { NavbarComponent } from './navbar/navbar/navbar.component';
import { ScheduleService } from './common/services/schedule.service';
import { HttpClientModule } from '@angular/common/http';
import { FakeScheduleService } from './common/services/fake-schedule.service';
import { RouterModule, Routes } from '@angular/router';
import { ScheduleTableComponent} from './schedule/schedule-table/schedule-table.component';
import { ScheduleResolver } from './common/resolvers/schedule.resolver';
import { NurseScheduleComponent } from './schedule/nurse-schedule/nurse-schedule.component';

const appRoutes: Routes = [
  {
    path: '',
    redirectTo: '/schedule',
    pathMatch: 'full'
  },
  { path: 'schedule',
    component: ScheduleTableComponent,
  },
  {
    path: 'nurses',
    component: NurseScheduleComponent,
    resolve: { schedule : ScheduleResolver }
  }
];


@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes),
    BrowserModule,
    HttpClientModule,
  ],
  declarations: [
    AppComponent,
    OneDayScheduleComponent,
    NavbarComponent,
    ScheduleTableComponent,
    NurseScheduleComponent,
  ],
  providers: [
    { provide: ScheduleService, useClass: FakeScheduleService },
    ScheduleResolver,
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }


