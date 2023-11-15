import {CanActivate, CanActivateFn, Router} from '@angular/router';
import {Injectable} from "@angular/core";


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(): boolean {
    const token = localStorage.getItem('SessionToken');

    if (token) {
      // Optionally, you can add more logic here to validate the token.
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}
