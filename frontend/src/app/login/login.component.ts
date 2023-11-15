import { Component } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormsModule } from '@angular/forms';
import {environment} from "../../Environments/environment";

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

  constructor(private http: HttpClient, private snackBar: MatSnackBar) { }

  onLogin() {
    const url = `${environment.baseUrl}/Auth/login`;

    this.http.post(url, this.loginData).subscribe(
      response => {
        this.snackBar.open('Login successful', 'Close', { duration: 3000 });
      },
      error => {
        this.snackBar.open('Login failed', 'Close', { duration: 3000 });
      }
    );
  }
}
