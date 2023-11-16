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
}
