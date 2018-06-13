import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';
import { OneDayScheduleComponent } from './schedule/one-day-schedule/one-day-schedule.component';
import { NavbarComponent } from './navbar/navbar/navbar.component';
import { ScheduleService } from './schedule.service';


@NgModule({
  declarations: [
    AppComponent,
    OneDayScheduleComponent,
    NavbarComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule,
    HttpModule
  ],
  providers: [ScheduleService],
  bootstrap: [AppComponent]
})
export class AppModule { }
