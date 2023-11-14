import {Component, EventEmitter, Output} from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  isCartVisible = false;
  @Output() aboutUsClick = new EventEmitter<void>();

  toggleCart() {
    this.isCartVisible = !this.isCartVisible;
  }

  aboutUsClicked() {
    this.aboutUsClick.emit();
  }
}
