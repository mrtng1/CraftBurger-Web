import { Component, OnInit } from '@angular/core';
import { BurgerService } from '../burger.service';
import { FriesService } from '../fries.service';

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

  addToCart(burger: any) {
    let cart = sessionStorage.getItem('cart');
    let cartArray;

    if (cart) {
      cartArray = JSON.parse(cart);
    } else {
      cartArray = [];
    }

    cartArray.push(burger);
    sessionStorage.setItem('cart', JSON.stringify(cartArray));
  }

  addToCartFries(fries: any) {
    let cart = sessionStorage.getItem('cart');
    let cartArray;

    if (cart) {
      cartArray = JSON.parse(cart);
    } else {
      cartArray = [];
    }

    cartArray.push(fries);
    sessionStorage.setItem('cart', JSON.stringify(cartArray));
    console.log('Cart Items:', cartArray);
  }

  getImageUrl(burgerName: string): string {
    return this.burgerService.getImageUrl(burgerName);
  }

  scrollToFries(): void {
    document.getElementById('fries')?.scrollIntoView({ behavior: 'smooth' });
  }
}