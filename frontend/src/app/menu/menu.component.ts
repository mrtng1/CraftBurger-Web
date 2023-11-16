import { Component, OnInit } from '@angular/core';
import { BurgerService } from '../service/burger.service';
import { FriesService } from '../service/fries.service';
import { CartItem } from '../models/CartItem';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  burgers: any[] = [];
  fries: any[] = [];
  currentIndex: number = 0;

  constructor(private burgerService: BurgerService, public friesService: FriesService) {}

  ngOnInit() {
    this.loadBurgers();
    this.loadFries();
  }

  loadBurgers() {
    this.burgerService.getBurgers().subscribe({
      next: (data) => {
        this.burgers = data.map(burger => ({
          ...burger,
          imageUrl: this.burgerService.getImageUrl(burger.burgerName)
        }));
      },
      error: (error) => console.error('Error fetching burgers:', error)
    });
  }

  loadFries() {
    this.friesService.getFries().subscribe({
      next: (data) => {
        this.fries = data.map(fry => ({
          ...fry,
          imageUrl: this.friesService.getImageUrl(fry.friesName)
        }));
      },
      error: (error) => console.error('Error fetching fries:', error)
    });
  }

  getBurgerDetails(burgerId: number) {
    this.burgerService.getBurgerDetails(burgerId);
  }

  addToCart(item: CartItem, itemType: string) {
    let cart = sessionStorage.getItem('cart');
    let cartArray: CartItem[];

    if (cart) {
      cartArray = JSON.parse(cart) as CartItem[];
    } else {
      cartArray = [];
    }

    const existingItemIndex = cartArray.findIndex((cartItem: CartItem) => cartItem.id === item.id);

    if (existingItemIndex !== -1) {
      // Increment quantity if the item exists
      cartArray[existingItemIndex].quantity = (cartArray[existingItemIndex].quantity || 0) + 1;
    } else {
      // Add new item with quantity 1
      const newItem = {...item, quantity: 1};
      cartArray.push(newItem);
    }

    sessionStorage.setItem('cart', JSON.stringify(cartArray));
    console.log(`${itemType} added to cart:`, cartArray);
  }


  getImageUrl(burgerName: string): string {
    return this.burgerService.getImageUrl(burgerName);
  }

  scrollToFries(): void {
    document.getElementById('fries')?.scrollIntoView({ behavior: 'smooth' });
  }
}
