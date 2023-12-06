import { Component } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from "@angular/router";
import { environment } from "../../Environments/environment";
import {CommonModule} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";

@Component({
  selector: 'app-register',
  standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerData = {
    fullName: '',
    email: '',
    password: '',
    confirmPassword: ''
  };

  constructor(private http: HttpClient, private snackBar: MatSnackBar, private router: Router) { }

  onRegister() {
    if (!this.isEmailValid(this.registerData.email)) {
      this.snackBar.open('Invalid email', 'Close', { duration: 3000 });
      return;
    }

    if (this.registerData.password !== this.registerData.confirmPassword) {
      this.snackBar.open('Passwords do not match', 'Close', { duration: 3000 });
      return;
    }

    const url = `${environment.baseUrl}/Auth/create`;
    this.http.post(url, { username: this.registerData.fullName, email: this.registerData.email, password: this.registerData.password }, { responseType: 'text' })
      .subscribe(
        response => {
          this.snackBar.open('Registration successful', 'Close', { duration: 3000 });
          this.router.navigate(['/login']);
        },
        error => {
          console.error('Registration error:', error);
          this.snackBar.open(`Registration failed: ${error.error}`, 'Close', { duration: 3000 });
        }
      );
  }

  private isEmailValid(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }
}
