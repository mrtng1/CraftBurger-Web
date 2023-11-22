import { Component } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent {
  showSubMenu: boolean = false;

  create(): void {
    console.log("create");
  }

  edit(): void {
    console.log("edit");
  }

  delete(): void {
    console.log("delete");
  }

  viewIngredients(): void {
    console.log("view");
  }

  toggleSubMenu(): void {
    this.showSubMenu = !this.showSubMenu;
  }
}
