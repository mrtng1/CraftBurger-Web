import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'frontend';

  scrollToAboutUs(): void {
    const aboutUsSection = document.getElementById('aboutUsSection');
    if (aboutUsSection) {
      aboutUsSection.scrollIntoView({behavior: 'smooth'});
    }
  }
}
