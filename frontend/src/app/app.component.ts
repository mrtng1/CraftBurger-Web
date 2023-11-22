import {Component, OnInit} from '@angular/core';
import {InitializationService} from "./service/initialization.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit{
  constructor(private initializationService: InitializationService) {}

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
