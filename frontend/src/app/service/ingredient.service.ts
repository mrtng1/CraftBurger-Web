
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {environment} from "../../Environments/environment";

@Injectable({
  providedIn: 'root'
})
export class IngredientService {
  constructor(private http: HttpClient) {}

  getIngredientsByBurgerId(burgerId: number): Observable<any[]> {
    return this.http.get<any[]>(`${environment.baseUrl}/api/burger/${burgerId}/ingredients`);
  }
}
