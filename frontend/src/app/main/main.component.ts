import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { ImageService } from "../service/image.service";
import { trigger, state, style, transition, animate } from '@angular/animations';
import {BurgerService} from "../service/burger.service";
import {CartService} from "../service/cart.service";
import {CartItem} from "../models/CartItem";

@Component({
    selector: 'app-main',
    templateUrl: './main.component.html',
    styleUrls: ['./main.component.css'],
    animations: [
      trigger('scaleIn', [
        transition(':enter', [
          style({ transform: 'scale(0.8)', opacity: 0 }),
          animate('200ms ease-out', style({ transform: 'scale(1)', opacity: 1 }))
        ])
      ])
    ]
})

export class MainComponent implements OnInit {
    burgers: any[] = [];
    currentIndex: number = 0;

    constructor(private burgerService: BurgerService, private imageService: ImageService, private cartService: CartService) {}

    ngOnInit() {
        this.burgerService.getBurgers().subscribe({
            next: (data) => {
                this.burgers = data.map(burger => ({
                    ...burger,
                    imageUrl: this.imageService.getImageUrl(burger.burgerName, "burger")
                }));
            },
            error: (error) => {
                console.error('Error fetching burgers:', error);
            }
        });
    }

    nextBurger() {
        this.currentIndex = (this.currentIndex + 1) % this.burgers.length;
    }

    previousBurger() {
        this.currentIndex =
            (this.currentIndex - 1 + this.burgers.length) % this.burgers.length;
    }

    get previousIndex(): number {
        return this.currentIndex === 0 ? this.burgers.length - 1 : this.currentIndex - 1;
    }

    get nextIndex(): number {
        return this.currentIndex === this.burgers.length - 1 ? 0 : this.currentIndex + 1;
    }

  addToCart(burger: CartItem) {
    let cart = sessionStorage.getItem('cart');
    let cartArray: CartItem[];

    if (cart) {
      cartArray = JSON.parse(cart) as CartItem[];
    } else {
      cartArray = [];
    }

    const existingItemIndex = cartArray.findIndex((cartItem: CartItem) => cartItem.id === burger.id);

    if (existingItemIndex !== -1) {
      cartArray[existingItemIndex].quantity = (cartArray[existingItemIndex].quantity || 0) + 1;
    } else {
      const newItem: CartItem = { ...burger, quantity: 1 };
      cartArray.push(newItem);
    }

    sessionStorage.setItem('cart', JSON.stringify(cartArray));
    this.cartService.updateCartCount(cartArray.length);
  }
}
