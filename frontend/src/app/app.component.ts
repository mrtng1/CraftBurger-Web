import {Component, OnInit} from '@angular/core';
import {InitializationService} from "./service/initialization.service";
import {filter, map, mergeMap} from "rxjs";
import {ActivatedRoute, NavigationEnd, Router} from "@angular/router";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit{
  showHeader = true;
  constructor(private initializationService: InitializationService, private router: Router, private activatedRoute: ActivatedRoute) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => this.activatedRoute),
      map(route => {
        while (route.firstChild) route = route.firstChild;
        return route;
      }),
      mergeMap(route => route.data)
    ).subscribe(data => this.showHeader = data['showHeader']); // Accessing using bracket notation
  }

  ngOnInit() {
    this.initializationService.initializeCartCount();
  }

  title = 'frontend';

  scrollToAboutUs(): void {
    const aboutUsSection = document.getElementById('aboutUsSection');
    if (aboutUsSection) {
      aboutUsSection.scrollIntoView({behavior: 'smooth'});
    }
  }
}
