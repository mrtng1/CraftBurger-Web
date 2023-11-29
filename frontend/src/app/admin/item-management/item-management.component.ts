import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MenuItem } from '../../models/MenuItem';
import { BurgerService } from '../../service/burger.service';
import { FriesService } from '../../service/fries.service';
import { forkJoin } from 'rxjs';

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

  constructor(private burgerService: BurgerService, private friesService: FriesService) {}

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
          type: 'Burger',
          description: burger.burgerDescription
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

  selectItem(item: MenuItem): void {
    this.selectedItem = { ...item };
    this.isEditable = false;
  }

  createItem(): void {
    this.selectedItem = { id: null, name: '', price: 0.01, description: '', type: 'Burger' };
    this.isEditable = true;
  }

  editItem(): void {
    this.isEditable = true;
  }

  deleteItem(): void {
    if (this.selectedItem && this.selectedItem.id) {
      if (this.selectedItem.type === 'Burger') {
        this.burgerService.deleteBurger(this.selectedItem.id).subscribe(() => this.fetchMenuItems());
      } else if (this.selectedItem.type === 'Fries') {
        this.friesService.deleteFries(this.selectedItem.id).subscribe(() => this.fetchMenuItems());
      }
    }
  }

  onFileSelected(event: Event): void {
    const element = event.currentTarget as HTMLInputElement;
    let fileList: FileList | null = element.files;
    if (fileList) {
      this.selectedItem.imageFile = fileList[0];
    } else {
      this.selectedItem.imageFile = null;
    }
  }

  saveItem(): void {
    if (this.isEditable) {
      const formData = new FormData();
      formData.append('name', this.selectedItem.name);
      formData.append('price', this.selectedItem.price.toString());
      formData.append('description', this.selectedItem.description);
      if (this.selectedItem.imageFile) {
        formData.append('image', this.selectedItem.imageFile, this.selectedItem.imageFile.name);
      }

      if (this.selectedItem.type === 'Burger') {
        if (this.selectedItem.id) {
          this.burgerService.updateBurger(this.selectedItem.id, formData).subscribe(() => this.fetchMenuItems());
        } else {
          this.burgerService.createBurger(formData).subscribe(() => this.fetchMenuItems());
        }
      } else if (this.selectedItem.type === 'Fries') {
        if (this.selectedItem.id) {
          this.friesService.updateFries(this.selectedItem.id, formData).subscribe(() => this.fetchMenuItems());
        } else {
          this.friesService.createFries(formData).subscribe(() => this.fetchMenuItems());
        }
      }

      this.isEditable = false;
      this.selectedItem = {};
    }
  }
}
