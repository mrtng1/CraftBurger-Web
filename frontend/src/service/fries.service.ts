import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../Environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FriesService {
  constructor(private http: HttpClient) {
  }

  getFries(): Observable<any[]> {
    const url = `${environment.baseUrl}/api/fries`;
    return this.http.get<any[]>(url);
  }

  getFriesById(friesId: number): Observable<any> {
    const url = `${environment.baseUrl}/api/fries/${friesId}`;
    return this.http.get<any>(url);
  }

  createFries(friesData: FormData): Observable<any> {
    const url = `${environment.baseUrl}/api/fries`;
    return this.http.post(url, friesData, {headers: this.getHeaders()});
  }

  updateFries(id: number, friesData: FormData): Observable<any> {
    const url = `${environment.baseUrl}/api/fries/${id}`;
    return this.http.put(url, friesData, {headers: this.getHeaders()});
  }

  deleteFries(friesId: number): Observable<any> {
    const url = `${environment.baseUrl}/api/fries/${friesId}`;
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
