import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";
import { MenuItem } from "../../models/MenuItem";
import { BurgerService } from '../../service/burger.service';
import { FriesService } from '../../service/fries.service';
import {forkJoin} from "rxjs";
import {MatDialog} from "@angular/material/dialog";
import {ConfirmationDialogComponent} from "../../confirmation-dialog/confirmation-dialog.component";
import {DipService} from "../../service/dip.service";

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
  isCreating: boolean = false;

  constructor(private burgerService: BurgerService,
              private friesService: FriesService,
              private dipService: DipService,
              private dialog: MatDialog,) {}

  ngOnInit() {
    this.fetchMenuItems();
  }

  fetchMenuItems(): void {
    forkJoin({
      burgers: this.burgerService.getBurgers(),
      fries: this.friesService.getFries(),
      dips: this.dipService.getDips()
    }).subscribe(({ burgers, fries, dips }) => {
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
        })),
        ...dips.map(dip => ({
          id: dip.id,
          name: dip.dipName,
          price: dip.dipPrice,
          type: 'Dip'
        }))
      ];
    });
  }


  selectItem(item: MenuItem): void {
    this.selectedItem = item;
    this.isEditable = true;
    this.isCreating = false;
  }

  createItem(): void {
    this.selectedItem = {
      id: null,
      name: '',
      price: 0.01,
      description: ''
    };
    this.isEditable = true;
    this.isCreating = true;
  }

  editItem(): void {
    this.isEditable = true;
    this.isCreating = false;
  }

  deleteItem(): void {
    if (this.selectedItem && this.selectedItem.id) {
      const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
        width: '250px',
        data: 'Are you sure you want to delete this item?'
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          switch (this.selectedItem.type) {
            case 'Burger':
              this.burgerService.deleteBurger(this.selectedItem.id).subscribe(() => this.fetchMenuItems());
              break;
            case 'Fries':
              this.friesService.deleteFries(this.selectedItem.id).subscribe(() => this.fetchMenuItems());
              break;
            case 'Dip':
              this.dipService.deleteDip(this.selectedItem.id).subscribe(() => this.fetchMenuItems());
              break;
            default:
              console.error('Unknown item type for deletion');
          }
        }
        this.isEditable = false;
        this.isCreating = false;
      });
    }
  }


  saveItem(): void {
    if (this.isEditable) {
      const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
        width: '250px',
        data: 'Are you sure you want to save these changes?'
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          if (this.selectedItem.id) {
          } else {
            this.createItemBasedOnType(this.selectedItem);
          }
          this.fetchMenuItems();
          this.isEditable = false;
          this.selectedItem = {};
          this.isCreating = false;
        }
      });
    }
  }
  createItemBasedOnType(item: any): void {
    switch (item.type) {
      case 'Burger':
        this.burgerService.createBurger({
          burgerName: item.name,
          burgerPrice: item.price,
          burgerDescription: item.description
        }).subscribe(() => this.fetchMenuItems());
        break;
      case 'Fries':
        this.friesService.createFries({
          friesName: item.name,
          friesPrice: item.price
        }).subscribe(() => this.fetchMenuItems());
        break;
      case 'Dip':
        this.dipService.createDip({
          dipName: item.name,
          dipPrice: item.price
        }).subscribe(() => this.fetchMenuItems());
        break;
      default:
        console.error('Unknown item type');
    }
  }


}
