import { Component } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormsModule } from '@angular/forms';
import {environment} from "../../Environments/environment";
import {Router} from "@angular/router";
import {UserService} from "../../service/user.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginData = {
    username: '',
    password: ''
  };

  constructor(private http: HttpClient, private snackBar: MatSnackBar, private router: Router, private userService: UserService) { }

  onLogin() {
    this.userService.login(this.loginData).subscribe(
      response => {
        if (response.token) {
          localStorage.setItem('SessionToken', response.token);
          this.snackBar.open('Login successful', 'Close', { duration: 3000 });
          this.router.navigate(['/admin']); // Navigate to /admin
        } else {
          this.snackBar.open('Login failed', 'Close', { duration: 3000 });
        }
      },
      error => {
        this.snackBar.open('Login failed', 'Close', { duration: 3000 });
      }
    );
  }
}
