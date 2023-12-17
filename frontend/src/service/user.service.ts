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

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/getAllUsers`, { headers: this.getHeaders() });
  }

  login(loginDto: LoginDTO): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, loginDto);
  }

  createUser(createDto: CreateDTO): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, createDto);
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/deleteUser/${id}`, { headers: this.getHeaders(),  responseType: 'text' });
  }

  validateToken(tokenDto: TokenDTO): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/validateToken`, tokenDto);
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
