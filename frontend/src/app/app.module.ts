import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { OneDayScheduleComponent } from './schedule/one-day-schedule/one-day-schedule.component';
import { NavbarComponent } from './navbar/navbar/navbar.component';
import { ScheduleService } from './services/schedule.service';
import { HttpClientModule } from '@angular/common/http';
import { FakeScheduleService } from './services/fake-schedule.service';
import { RouterModule, Routes } from '@angular/router';
import { ScheduleTableComponent} from './schedule/schedule-table/schedule-table.component';

const appRoutes: Routes = [
  {
    path: '',
    redirectTo: '/schedule',
    pathMatch: 'full'
  },
  { path: 'schedule',
    component: ScheduleTableComponent,
  },
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
  ],
  providers: [
    { provide: ScheduleService, useClass: FakeScheduleService },
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }


