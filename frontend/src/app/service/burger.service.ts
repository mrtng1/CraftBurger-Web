import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import {environment} from "../../Environments/environment";

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
        return `/inspect/${burgerId}`;
    }

    getBurgerDetails(burgerId: number): void {
        const burgerDetailsUrl = this.getBurgerDetailsUrl(burgerId);
        this.router.navigate([burgerDetailsUrl]);
    }
  createBurger(burger: any): Observable<any> {
    const url = `${environment.baseUrl}/api/burger`;
    return this.http.post(url, burger);
  }

  updateBurger(burgerId: number, burger: any): Observable<any> {
    const url = `${environment.baseUrl}/api/burger/${burgerId}`;
    return this.http.put(url, burger);
  }

  deleteBurger(burgerId: number): Observable<any> {
    const url = `${environment.baseUrl}/api/burger/${burgerId}`;
    return this.http.delete(url);
  }

}
