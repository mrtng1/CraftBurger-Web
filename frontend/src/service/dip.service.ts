import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import {environment} from "../Environments/environment";
import {Dip} from "../models/Dip";

@Injectable({
  providedIn: 'root'
})
export class DipService {
  constructor(private http: HttpClient, private router: Router) {}

  getDips(): Observable<any[]> {
    const url = `${environment.baseUrl}/api/dips`;
    return this.http.get<any[]>(url);
  }

  getDipById(dipId: number): Observable<any> {
    const url = `${environment.baseUrl}/api/dip/${dipId}`;
    return this.http.get<any>(url);
  }

  getDipDetailsUrl(dipId: number): string {
    return `/inspect/${dipId}`;
  }

  getDipDetails(dipId: number): void {
    const dipDetailsUrl = this.getDipDetailsUrl(dipId);
    this.router.navigate([dipDetailsUrl]);
  }

    createDip(dipData: Dip): Observable<any> {
        return this.http.post(`${environment.baseUrl}/api/dip`, dipData);
    }

  updateDip(id: number, dipData: Dip): Observable<any> {
    const payload = {
      id: id,
      name: dipData.name,
      price: dipData.price
    };
    return this.http.put(`${environment.baseUrl}/api/dip/${id}`, payload, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }

  deleteDip(dipId: number): Observable<any> {
    const url = `${environment.baseUrl}/api/dip/${dipId}`;
    return this.http.delete(url);
  }
}
