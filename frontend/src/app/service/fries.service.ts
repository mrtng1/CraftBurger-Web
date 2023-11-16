import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../Environments/environment';
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class FriesService {
  constructor(private http: HttpClient, private router: Router) {}

  getFries(): Observable<any[]> {
    const url = `${environment.baseUrl}/api/fries`;
    return this.http.get<any[]>(url);
  }

  getFriesDetails(friesId: number): Observable<any> {
    const url = `${environment.baseUrl}/api/fries/${friesId}`;
    return this.http.get<any>(url);
  }

  addToCart(fries: any): void {
    let cart = sessionStorage.getItem('cart');
    let cartArray;

    if (cart) {
      cartArray = JSON.parse(cart);
    } else {
      cartArray = [];
    }

    cartArray.push(fries);
    sessionStorage.setItem('cart', JSON.stringify(cartArray));
  }

  getImageUrl(fryName: string): string {
    if (!fryName) {
      return '/path/to/default-image.jpg'; // Or any suitable default
    }
    const formattedName = fryName.toLowerCase().replace(/\s+/g, '-');
    return `/assets/fries/${formattedName}.JPG`;
  }

  // Additional utility methods for fries can be added here
}
