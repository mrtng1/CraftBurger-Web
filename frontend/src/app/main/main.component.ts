import {Component, OnInit} from '@angular/core';
import {ImageService} from "../../service/image.service";
import {animate, style, transition, trigger} from '@angular/animations';
import {BurgerService} from "../../service/burger.service";
import {CartService} from "../../service/cart.service";
import {CartItem} from "../../models/CartItem";

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css'],
  animations: [
    trigger('scaleIn', [
      transition(':enter', [
        style({transform: 'scale(0.8)', opacity: 0}),
        animate('200ms ease-out', style({transform: 'scale(1)', opacity: 1}))
      ])
    ])
  ]
})

export class MainComponent implements OnInit {
  burgers: any[] = [];
  currentIndex: number = 0;

  constructor(private burgerService: BurgerService, private imageService: ImageService, private cartService: CartService) {
  }

  get previousIndex(): number {
    return this.currentIndex === 0 ? this.burgers.length - 1 : this.currentIndex - 1;
  }

  get nextIndex(): number {
    return this.currentIndex === this.burgers.length - 1 ? 0 : this.currentIndex + 1;
  }

  ngOnInit() {
    this.loadBurgers();
  }

  loadBurgers() {
    this.burgerService.getBurgers().subscribe({
      next: (data) => {
        this.burgers = data;
      },
      error: (error) => console.error('Error fetching burgers:', error)
    });
  }

  nextBurger() {
    this.currentIndex = (this.currentIndex + 1) % this.burgers.length;
  }

  previousBurger() {
    this.currentIndex =
      (this.currentIndex - 1 + this.burgers.length) % this.burgers.length;
  }

  addToCart(burger: CartItem) {
    let cart = sessionStorage.getItem('cart');
    let cartArray: CartItem[] = cart ? JSON.parse(cart) : [];

    const existingItemIndex = cartArray.findIndex((cartItem: CartItem) => cartItem.id === burger.id && cartItem.itemType === 'burger');

    if (existingItemIndex !== -1) {
      cartArray[existingItemIndex].quantity = (cartArray[existingItemIndex].quantity || 0) + 1;
    } else {
      const newItem: CartItem = {
        ...burger,
        quantity: 1,
        itemType: 'burger'
      };
      cartArray.push(newItem);
    }

    sessionStorage.setItem('cart', JSON.stringify(cartArray));
    this.cartService.updateCartCount(cartArray.length);
  }
}
