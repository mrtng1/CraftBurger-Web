import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpClient} from "@angular/common/http";
import {MenuItem} from "../../models/MenuItem";
import { BurgerService } from '../../service/burger.service';
import { FriesService } from '../../service/fries.service';

@Component({
  selector: 'app-item-management',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './item-management.component.html',
  styleUrl: './item-management.component.css'
})
export class ItemManagementComponent {
  menuItems: MenuItem[] = [];

  constructor(private burgerService: BurgerService,
              private friesService: FriesService) {}

  ngOnInit() {
    this.fetchMenuItems();
  }

  fetchMenuItems(): void {
    this.burgerService.getBurgers().subscribe(burgers => {
      const burgerItems: MenuItem[] = burgers.map(burger => ({
        id: burger.id, // Ensure the property names match
        name: burger.burgerName,
        price: burger.burgerPrice,
        type: 'Burger'
      }));
      this.menuItems = [...this.menuItems, ...burgerItems];
    });

    this.friesService.getFries().subscribe(fries => {
      const friesItems: MenuItem[] = fries.map(fry => ({
        id: fry.id, // Ensure the property names match
        name: fry.friesName,
        price: fry.friesPrice,
        type: 'Fries'
      }));
      this.menuItems = [...this.menuItems, ...friesItems];
    });
  }
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
