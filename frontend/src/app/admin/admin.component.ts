import { Component } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent {
  showSubMenu: boolean = false;

  viewUserManagement(): void {
    console.log("User Management");
  }

  viewItemManagement(): void {
    console.log("Item Management");
  }

  viewOverview(): void {
    console.log("Overview");
  }

  toggleSubMenu(): void {
    this.showSubMenu = !this.showSubMenu;
  }
}
