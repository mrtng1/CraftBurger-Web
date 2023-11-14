import { Component, OnInit } from '@angular/core';
import {BurgerService} from "../burger.service";

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  burgers: any[] = [];
  currentIndex: number = 0;

  constructor(private burgerService: BurgerService) {}

  ngOnInit() {
    this.burgerService.getBurgers().subscribe({
      next: (data) => {
        this.burgers = data.map(burger => ({
          ...burger,
          imageUrl: this.burgerService.getImageUrl(burger.burgerName)
        }));
      },
      error: (error) => {
        console.error('Error fetching burgers:', error);
      }
    });
  }

  getBurgerDetails(burgerId: number) {
    this.burgerService.getBurgerDetails(burgerId);
  }

  addToCart(burger: any) {
    this.burgerService.addToCart(burger);
  }

  getImageUrl(burgerName: string): string {
    return this.burgerService.getImageUrl(burgerName);
  }
}
