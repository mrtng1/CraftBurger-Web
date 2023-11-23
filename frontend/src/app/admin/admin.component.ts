import { Component } from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent {
  showSubMenu: boolean = false;

  constructor(private router: Router) {}
  viewUserManagement(): void {
    this.router.navigate(['/admin/user-management']);
  }

  viewItemManagement(): void {
    this.router.navigate(['/admin/item-management']);
  }

  viewOverview(): void {
    this.router.navigate(['/admin/overview']);
  }

  toggleSubMenu(): void {
    this.showSubMenu = !this.showSubMenu;
  }
}
