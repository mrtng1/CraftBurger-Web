import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-item-management',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './item-management.component.html',
  styleUrl: './item-management.component.css'
})
export class ItemManagementComponent {
  createItem(): void {
    console.log("Create Item clicked");
  }

  editItem(): void {
    console.log("Edit Item clicked");
  }

  deleteItem(): void {
    console.log("Delete Item clicked");
  }
}
