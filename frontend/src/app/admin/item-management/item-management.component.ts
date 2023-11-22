import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";
import { MenuItem } from "../../models/MenuItem";
import { BurgerService } from '../../service/burger.service';
import { FriesService } from '../../service/fries.service';
import { IngredientService } from "../../service/ingredient.service";
import { Ingredient } from "../../models/Ingredient";
import {forkJoin} from "rxjs";

@Component({
  selector: 'app-item-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './item-management.component.html',
  styleUrls: ['./item-management.component.css']
})
export class ItemManagementComponent implements OnInit {
  menuItems: MenuItem[] = [];
  selectedItem: MenuItem | any = {};
  isEditable: boolean = false;

  ingredients: Ingredient[] = [];

  constructor(private burgerService: BurgerService,
              private friesService: FriesService,
              private ingredientService: IngredientService) {}

  ngOnInit() {
    this.fetchMenuItems();
  }

  fetchMenuItems(): void {
    forkJoin({
      burgers: this.burgerService.getBurgers(),
      fries: this.friesService.getFries()
    }).subscribe(({ burgers, fries }) => {
      this.menuItems = [
        ...burgers.map(burger => ({
          id: burger.id,
          name: burger.burgerName,
          price: burger.burgerPrice,
          type: 'Burger'
        })),
        ...fries.map(fry => ({
          id: fry.id,
          name: fry.friesName,
          price: fry.friesPrice,
          type: 'Fries'
        }))
      ];
    });
  }

  loadIngredientsForBurger(burgerId: number): void {
    this.ingredientService.getIngredientsByBurgerId(burgerId).subscribe(
      data => {
        this.ingredients = data;
      },
      error => {
        console.error('Error fetching data:', error);
      }
    );
  }

  selectItem(item: MenuItem): void {
    this.selectedItem = item;
    this.isEditable = false;
    if (item.type === 'Burger') {
      this.loadIngredientsForBurger(item.id);
    } else {
      this.ingredients = [];
    }
  }

  createItem(): void {
    this.selectedItem = {
      id: null,
      burgerName: '',
      burgerPrice: 0.01
    };
    this.isEditable = true;
  }

  editItem(): void {
    this.isEditable = true;
  }

  deleteItem(): void {
    if (this.selectedItem && this.selectedItem.id) {
      this.burgerService.deleteBurger(this.selectedItem.id).subscribe(() => {
        this.fetchMenuItems();
      });
    }
  }

  saveItem(): void {
    if (this.isEditable) {
      const burgerData = {
        id: this.selectedItem.id || 0,
        burgerName: this.selectedItem.name,
        burgerPrice: this.selectedItem.price
      };

      if (burgerData.id) {
        this.burgerService.updateBurger(burgerData.id, burgerData).subscribe(() => {
          this.fetchMenuItems();
        });
      } else {
        this.burgerService.createBurger(burgerData).subscribe(() => {
          this.fetchMenuItems();
        });
      }
      this.isEditable = false;
    }
  }
}
