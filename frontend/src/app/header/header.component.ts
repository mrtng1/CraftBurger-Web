import {Component, EventEmitter, Output} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  isCartVisible = false;
  @Output() aboutUsClick = new EventEmitter<void>();

  constructor(private router: Router, private route: ActivatedRoute) { }

  toggleCart() {
    this.isCartVisible = !this.isCartVisible;
  }

  aboutUsClicked() {
    if (this.route.snapshot.routeConfig?.path === 'home') {
      this.aboutUsClick.emit();
    } else {
      this.router.navigate(['/home']).then(() => {
        this.aboutUsClick.emit();
      });
    }
  }
}
