import {CanActivate, CanActivateFn, Router} from '@angular/router';
import {Injectable} from "@angular/core";
import {map} from "rxjs";
import {environment} from "../Environments/environment";
import {HttpClient} from "@angular/common/http";


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private http: HttpClient) {}

  canActivate(): boolean {
    const token = localStorage.getItem('SessionToken');

    if (!token) {
      this.router.navigate(['/login']);
      return false;
    }

    // Call backend to validate the token
    this.http.post<boolean>(`${environment.baseUrl}/Auth/validateToken`, { Token: token })
      .pipe(map(isValid => {
        if (!isValid) {
          this.router.navigate(['/login']);
        }
        return isValid;
      })).subscribe();

    // This might need refinement for asynchronous handling
    return true;
  }
}
