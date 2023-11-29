import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from "../../Environments/environment";

@Injectable({
  providedIn: 'root'
})
export class BurgerService {
  constructor(private http: HttpClient) {}

  getBurgers(): Observable<any[]> {
    const url = `${environment.baseUrl}/api/burgers`;
    return this.http.get<any[]>(url);
  }

  createBurger(burgerData: FormData): Observable<any> {
    const url = `${environment.baseUrl}/api/burger`;
    return this.http.post(url, burgerData);
  }

  updateBurger(burgerId: number, burgerData: FormData): Observable<any> {
    const url = `${environment.baseUrl}/api/burger/${burgerId}`;
    return this.http.put(url, burgerData);
  }

  deleteBurger(burgerId: number): Observable<any> {
    const url = `${environment.baseUrl}/api/burger/${burgerId}`;
    return this.http.delete(url);
  }
}
