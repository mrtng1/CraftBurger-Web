import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from "../Environments/environment";
import {User} from "../models/User";
import {LoginDTO} from "../models/LoginDTO";
import {CreateDTO} from "../models/CreateDTO";
import {TokenDTO} from "../models/TokenDTO";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${environment.baseUrl}/auth`;

  constructor(private http: HttpClient) {}

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/getUserById/${id}`);
  }

  login(loginDto: LoginDTO): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, loginDto);
  }

  createUser(createDto: CreateDTO): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, createDto);
  }

  validateToken(tokenDto: TokenDTO): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/validateToken`, tokenDto);
  }

  private getHeaders(): HttpHeaders {
    let headers = new HttpHeaders();
    // Add any common headers you need, for example, for authentication
    return headers;
  }
}
