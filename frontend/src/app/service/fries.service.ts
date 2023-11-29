import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../Environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FriesService {
  constructor(private http: HttpClient) {}

  getFries(): Observable<any[]> {
    const url = `${environment.baseUrl}/api/fries`;
    return this.http.get<any[]>(url);
  }

  createFries(friesData: FormData): Observable<any> {
    const url = `${environment.baseUrl}/api/fries`;
    return this.http.post(url, friesData);
  }

  updateFries(friesId: number, friesData: FormData): Observable<any> {
    const url = `${environment.baseUrl}/api/fries/${friesId}`;
    return this.http.put(url, friesData);
  }

  deleteFries(friesId: number): Observable<any> {
    const url = `${environment.baseUrl}/api/fries/${friesId}`;
    return this.http.delete(url);
  }
}
