import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from "../Environments/environment";

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  constructor(private http: HttpClient) {
  }

  createOrder(orderData: any): Observable<any> {
    const url = `${environment.baseUrl}/api/order`;
    return this.http.post(url, orderData);
  }

  getAllUserOrders(): Observable<any> {
    const url = `${environment.baseUrl}/api/userOrders`;
    return this.http.get(url);
  }

  getAllOrderDetails(): Observable<any> {
    const url = `${environment.baseUrl}/api/orderDetails`;
    return this.http.get(url);
  }

  getOrderById(orderId: number): Observable<any> {
    const url = `${environment.baseUrl}/api/order/${orderId}`;
    return this.http.get(url);
  }
}
