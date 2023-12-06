import {CanActivate, CanActivateFn, Router} from '@angular/router';
import {Injectable} from "@angular/core";
import {map, Observable, of} from "rxjs";
import {environment} from "../Environments/environment";
import {HttpClient} from "@angular/common/http";


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private http: HttpClient) {}

  canActivate(): Observable<boolean> {
    const token = localStorage.getItem('SessionToken');
    if (!token) {
      this.router.navigate(['/login']);
      return of(false);
    }

    const decodedToken = this.decodeToken(token);

    // Check if 'IsAdmin' claim is 'true'
    if (decodedToken.IsAdmin !== 'true') {
      this.router.navigate(['/home']);
      return of(false);
    }

    return this.http.post<boolean>(`${environment.baseUrl}/Auth/validateToken`, { Token: token }).pipe(
      map(isValid => {
        if (!isValid) {
          this.router.navigate(['/login']);
        }
        return isValid;
      })
    );
  }

  private decodeToken(token: string): any {
    try {
      const payload = token.split('.')[1];
      const decodedJson = atob(payload);
      return JSON.parse(decodedJson);
    } catch (e) {
      console.error('Error decoding token', e);
      return null;
    }
  }
}
