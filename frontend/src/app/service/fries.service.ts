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
  createFries(friesData: any): Observable<any> {
    const url = `${environment.baseUrl}/api/fries`;
    return this.http.post(url, friesData);
  }
  updateFries(friesId: number, friesData: any): Observable<any> {
    const url = `${environment.baseUrl}/api/fries/${friesId}`;
    return this.http.put(url, friesData);
  }
  deleteFries(friesId: number): Observable<any> {
    const url = `${environment.baseUrl}/api/fries/${friesId}`;
    return this.http.delete(url);
  }
}
