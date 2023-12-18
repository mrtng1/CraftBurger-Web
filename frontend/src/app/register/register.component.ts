import {Component} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MatSnackBar} from '@angular/material/snack-bar';
import {Router} from "@angular/router";
import {CommonModule} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {UserService} from "../../service/user.service";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  registerData = {
    fullName: '',
    email: '',
    password: '',
    confirmPassword: ''
  };

  constructor(private http: HttpClient, private snackBar: MatSnackBar, private router: Router, private userService: UserService) {
  }

  onRegister() {
    if (!this.isEmailValid(this.registerData.email)) {
      this.snackBar.open('Invalid email', 'Close', {duration: 3000});
      return;
    }

    if (this.registerData.password !== this.registerData.confirmPassword) {
      this.snackBar.open('Passwords do not match', 'Close', {duration: 3000});
      return;
    }

    this.userService.createUser({
      username: this.registerData.fullName,
      email: this.registerData.email,
      password: this.registerData.password
    }).subscribe(
      response => {
        this.snackBar.open('Registration successful', 'Close', {duration: 3000});
        this.router.navigate(['/login']);
      },
      error => {
        console.error('Registration error:', error);
        this.snackBar.open(`Registration failed: ${error.error}`, 'Close', {duration: 3000});
      }
    );
  }

  private isEmailValid(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }
}
