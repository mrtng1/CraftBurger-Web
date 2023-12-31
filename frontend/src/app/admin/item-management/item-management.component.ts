import {Component, OnInit} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {MenuItem} from '../../../models/MenuItem';
import {BurgerService} from '../../../service/burger.service';
import {FriesService} from '../../../service/fries.service';
import {forkJoin} from 'rxjs';
import {MatSnackBar} from "@angular/material/snack-bar";
import {DipService} from "../../../service/dip.service";
import {Dip} from "../../../models/Dip";

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

  constructor(
    private burgerService: BurgerService,
    private friesService: FriesService,
    private dipService: DipService,
    private snackBar: MatSnackBar
  ) {
  }

  ngOnInit() {
    this.fetchMenuItems();
  }

  fetchMenuItems(): void {
    forkJoin({
      burgers: this.burgerService.getBurgers(),
      fries: this.friesService.getFries(),
      dips: this.dipService.getDips()
    }).subscribe(({burgers, fries, dips}) => {
      this.menuItems = [
        ...burgers.map(burger => ({
          id: burger.id,
          name: burger.name,
          price: burger.price,
          type: 'Burger',
          description: burger.description,
          image: burger.image
        })),
        ...fries.map(fry => ({
          id: fry.id,
          name: fry.name,
          price: fry.price,
          type: 'Fries',
          description: fry.description,
          image: fry.image
        })),
        ...dips.map(dip => ({
          id: dip.id,
          name: dip.name,
          price: dip.price,
          type: 'Dip',
          description: dip.description,
          image: dip.image
        }))
      ];
    });
  }

  selectItem(item: MenuItem): void {
    this.selectedItem = {...item};
    this.isEditable = false;
  }

  createItem(): void {
    this.selectedItem = {id: null, name: '', price: '', description: '', type: ''};
    this.isEditable = true;
  }

  editItem(): void {
    this.isEditable = true;
  }

  deleteItem(): void {
    if (this.selectedItem && this.selectedItem.id) {
      if (this.selectedItem.type === 'Burger') {
        this.burgerService.deleteBurger(this.selectedItem.id).subscribe(() => {
          this.fetchMenuItems();
          this.snackBar.open('Burger deleted successfully!', 'Close', {duration: 3000});
        }, error => {
          this.snackBar.open('Error while deleting burger. Please try again.', 'Close', {duration: 3000});
        });
      } else if (this.selectedItem.type === 'Fries') {
        this.friesService.deleteFries(this.selectedItem.id).subscribe(() => {
          this.fetchMenuItems();
          this.snackBar.open('Fries deleted successfully!', 'Close', {duration: 3000});
        }, error => {
          this.snackBar.open('Error while deleting fries. Please try again.', 'Close', {duration: 3000});
        });
      } else if (this.selectedItem.type === 'Dip') {
        this.dipService.deleteDip(this.selectedItem.id).subscribe(() => {
          this.fetchMenuItems();
          this.snackBar.open('Dip deleted successfully!', 'Close', {duration: 3000});
        }, error => {
          this.snackBar.open('Error while deleting dip. Please try again.', 'Close', {duration: 3000});
        });
      }

      this.isEditable = false;
      this.selectedItem = {};
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

  resetFileInput(): void {
    let fileInput = <HTMLInputElement>document.getElementById('fileInput');
    if (fileInput) {
      fileInput.value = '';
    }
  }

  validateName(): string | null {
    if (!this.selectedItem.name) {
      return 'Name is required.';
    }
    if (this.selectedItem.name.length > 30) {
      return 'Name cannot exceed 30 characters.';
    }
    if (!/^[a-zA-Z ]+$/.test(this.selectedItem.name)) {
      return 'Name must contain letters and spaces only.';
    }
    return null;
  }

  validatePrice(): string | null {
    if (!this.selectedItem.price) {
      return 'Price is required.';
    }
    if (this.selectedItem.type === 'Burger' && (this.selectedItem.price < 70 || this.selectedItem.price > 200)) {
      return 'Price for a burger must be between 70 and 200.';
    }
    if (this.selectedItem.type === 'Fries' && (this.selectedItem.price < 10 || this.selectedItem.price > 100)) {
      return 'Price for fries must be between 10 and 100.';
    }
    if (this.selectedItem.type === 'Dip' && (this.selectedItem.price < 1 || this.selectedItem.price > 50)) {
      return 'Price for a dip must be between 1 and 50.';
    }
    return null;
  }

  validateDescription(): string | null {
    if (this.selectedItem.type !== 'Burger') {
      return null;
    }

    if (!this.selectedItem.description) {
      return 'Description is required for burgers.';
    }
    if (this.selectedItem.description.length > 500) {
      return 'Description cannot exceed 500 characters.';
    }
    return null;
  }

  saveItem(): void {
    if (this.isEditable) {
      const priceError = this.validatePrice();
      const nameError = this.validateName();

      const errorMessage = priceError ?? nameError;
      if (errorMessage) {
        this.snackBar.open(errorMessage, 'Close', {duration: 3000});
        return;
      }

      const formData = new FormData();
      formData.append('name', this.selectedItem.name);
      formData.append('price', this.selectedItem.price.toString());

      // Only add the image file if the item is not a dip
      if (this.selectedItem.type !== 'Dip' && this.selectedItem.imageFile) {
        formData.append('image', this.selectedItem.imageFile, this.selectedItem.imageFile.name);
      }

      // Handle item based on its type
      if (this.selectedItem.type === 'Burger') {
        this.handleBurgerItem(formData);
      } else if (this.selectedItem.type === 'Fries') {
        this.handleFriesItem(formData);
      } else if (this.selectedItem.type === 'Dip') {
        this.handleDipItem();
      }
    }
  }

  handleBurgerItem(formData: FormData): void {
    if (this.selectedItem.type === 'Burger') {
      formData.append('description', this.selectedItem.description);
    }

    if (this.selectedItem.id) {
      formData.append('id', this.selectedItem.id)
      this.burgerService.updateBurger(this.selectedItem.id, formData).subscribe(() => {
        this.fetchMenuItems();
        this.snackBar.open('Burger updated successfully!', 'Close', {duration: 4000});
        this.isEditable = false;
        this.selectedItem = {};
        this.resetFileInput();
      }, error => {
        this.snackBar.open('Error while updating burger. Please try again.', 'Close', {duration: 4000});
      });
    } else {
      this.burgerService.createBurger(formData).subscribe(() => {
        this.fetchMenuItems();
        this.snackBar.open('Burger created successfully!', 'Close', {duration: 4000});
        this.isEditable = false;
        this.selectedItem = {};
        this.resetFileInput();
      }, error => {
        this.snackBar.open('Error while creating burger. Please try again.', 'Close', {duration: 4000});
      });
    }
  }

  handleFriesItem(formData: FormData): void {
    if (this.selectedItem.id) {
      formData.append('id', this.selectedItem.id)
      this.friesService.updateFries(this.selectedItem.id, formData).subscribe(() => {
        this.fetchMenuItems();
        this.snackBar.open('Fries updated successfully!', 'Close', {duration: 3000});
        this.isEditable = false;
        this.selectedItem = {};
        this.resetFileInput();
      }, error => {
        this.snackBar.open('Error while updating fries. Please try again.', 'Close', {duration: 3000});
      });
    } else {
      this.friesService.createFries(formData).subscribe(() => {
        this.fetchMenuItems();
        this.snackBar.open('Fries created successfully!', 'Close', {duration: 3000});
        this.isEditable = false;
        this.selectedItem = {};
        this.resetFileInput()
      }, error => {
        this.snackBar.open('Error while creating fries. Please try again.', 'Close', {duration: 3000});
      });
    }
  }

  handleDipItem(): void {
    let dipData: Dip = {
      name: this.selectedItem.name,
      price: this.selectedItem.price
    };

    if (this.selectedItem.id) {
      dipData.id = this.selectedItem.id;
      this.dipService.updateDip(this.selectedItem.id, dipData).subscribe(() => {
        this.fetchMenuItems();
        this.snackBar.open('Dip updated successfully!', 'Close', {duration: 4000});
        this.isEditable = false;
        this.selectedItem = {};
      }, error => {
        this.snackBar.open('Error while updating dip. Please try again.', 'Close', {duration: 4000});
      });
    } else {
      this.dipService.createDip(dipData).subscribe(() => {
        this.fetchMenuItems();
        this.snackBar.open('Dip created successfully!', 'Close', {duration: 4000});
        this.isEditable = false;
        this.selectedItem = {};
      }, error => {
        this.snackBar.open('Error while creating dip. Please try again.', 'Close', {duration: 4000});
      });
    }
  }


}
