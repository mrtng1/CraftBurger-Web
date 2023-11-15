import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import {environment} from "../Environments/environment";

@Injectable({
    providedIn: 'root'
})
export class BurgerService {
    constructor(private http: HttpClient, private router: Router) {}

    getBurgers(): Observable<any[]> {
        const url = `${environment.baseUrl}/api/burgers`;

        return this.http.get<any[]>(url);
    }

    getBurgerDetailsUrl(burgerId: number): string {
        // Assuming you have a route for '/inspect/:id', where ':id' is the burgerId
        return `/inspect/${burgerId}`;
    }

    getImageUrl(burgerName: string): string {
        const formattedName = burgerName.toLowerCase().replace(/\s+/g, '-');
        return `/assets/burgers/${formattedName}.jpg`;
    }

    getBurgerDetails(burgerId: number): void {
        const burgerDetailsUrl = this.getBurgerDetailsUrl(burgerId);
        this.router.navigate([burgerDetailsUrl]);
    }

    addToCart(burger: any): void {
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
