import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { BurgerService } from "../service/burger.service";
import { trigger, state, style, transition, animate } from '@angular/animations';

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

    constructor(
        private router: Router,
        private burgerService: BurgerService // Using BurgerService
    ) {}

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
}
