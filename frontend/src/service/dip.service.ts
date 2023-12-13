import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import {environment} from "../Environments/environment";

@Injectable({
  providedIn: 'root'
})
export class DipService {
  constructor(private http: HttpClient, private router: Router) {}

  getDips(): Observable<any[]> {
    const url = `${environment.baseUrl}/api/dips`;
    return this.http.get<any[]>(url);
  }

  getDipDetailsUrl(dipId: number): string {
    return `/inspect/${dipId}`;
  }

  getDipDetails(dipId: number): void {
    const dipDetailsUrl = this.getDipDetailsUrl(dipId);
    this.router.navigate([dipDetailsUrl]);
  }

  createDip(dip: any): Observable<any> {
    const url = `${environment.baseUrl}/api/dip`;
    return this.http.post(url, dip);
  }

  updateDip(dipId: number, dip: any): Observable<any> {
    const url = `${environment.baseUrl}/api/dip/${dipId}`;
    return this.http.put(url, dip);
  }

  deleteDip(dipId: number): Observable<any> {
    const url = `${environment.baseUrl}/api/dip/${dipId}`;
    return this.http.delete(url);
  }
}
