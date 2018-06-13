import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'pz-navbar',
  templateUrl: './navbar.template.html',
  styleUrls: ['./navbar.styles.css']
})
export class NavbarComponent implements OnInit {


  constructor(private router: Router) {

  }

  ngOnInit() {
  }

  navigateTo(url): void {
    this.router.navigateByUrl(url);
  }
}
