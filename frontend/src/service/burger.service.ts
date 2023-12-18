import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from "../Environments/environment";

@Injectable({
  providedIn: 'root'
})
export class BurgerService {
  constructor(private http: HttpClient) {
  }

  getBurgers(): Observable<any[]> {
    const url = `${environment.baseUrl}/api/burgers`;
    return this.http.get<any[]>(url);
  }

  getBurgerById(burgerId: number): Observable<any> {
    const url = `${environment.baseUrl}/api/burger/${burgerId}`;
    return this.http.get<any>(url);
  }

  createBurger(burgerData: FormData): Observable<any> {
    const url = `${environment.baseUrl}/api/burger`;
    return this.http.post(url, burgerData, {headers: this.getHeaders()});
  }

  updateBurger(id: number, burgerData: FormData): Observable<any> {
    const url = `${environment.baseUrl}/api/burger/${id}`;
    return this.http.put(url, burgerData, {headers: this.getHeaders()});
  }

  deleteBurger(burgerId: number): Observable<any> {
    const url = `${environment.baseUrl}/api/burger/${burgerId}`;
    return this.http.delete(url, {headers: this.getHeaders()});
  }

  private getHeaders(): HttpHeaders {
    let headers = new HttpHeaders();
    const token = localStorage.getItem('SessionToken');
    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }
    return headers;
  }
}
