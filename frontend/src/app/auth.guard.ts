import {CanActivate, Router} from '@angular/router';
import {Injectable} from "@angular/core";
import {map, Observable, of} from "rxjs";
import {UserService} from "../service/user.service";


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private userService: UserService) {
  }

  canActivate(): Observable<boolean> {
    const token = localStorage.getItem('SessionToken');
    if (!token) {
      this.router.navigate(['/login']);
      return of(false);
    }

    const decodedToken = this.decodeToken(token);
    if (decodedToken.IsAdmin !== 'true') {
      this.router.navigate(['/home']);
      return of(false);
    }

    return this.userService.validateToken({token: token}).pipe(
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
