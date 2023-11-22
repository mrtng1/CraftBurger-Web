import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";
import { MenuItem } from "../../models/MenuItem";
import { BurgerService } from '../../service/burger.service';
import { FriesService } from '../../service/fries.service';
import { IngredientService } from "../../service/ingredient.service";
import { Ingredient } from "../../models/Ingredient";

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
    this.burgerService.getBurgers().subscribe(burgers => {
      this.menuItems = burgers.map(burger => ({
        id: burger.id,
        name: burger.burgerName,
        price: burger.burgerPrice,
        type: 'Burger'
      }));
    });

    this.friesService.getFries().subscribe(fries => {
      const friesItems = fries.map(fry => ({
        id: fry.id,
        name: fry.friesName,
        price: fry.friesPrice,
        type: 'Fries'
      }));
      this.menuItems = [...this.menuItems, ...friesItems];
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
      name: '',
      price: 0.0,
      type: ''
    };
    this.isEditable = true;
  }

  editItem(): void {
    this.isEditable = true;
  }

  deleteItem(): void {
    console.log("delete item");
  }

  saveItem(): void {
    if (this.isEditable) {
      console.log("save item");
    }
  }
}
