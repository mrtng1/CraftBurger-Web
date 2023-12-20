import {Component} from '@angular/core';
import {UserService} from "../../../service/user.service";

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent {
  users: any[] = [];
  selectedUserId: number | null = null;

  constructor(private userService: UserService) {
  }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getAllUsers().subscribe(data => {
      this.users = data;
    }, error => {
      console.error('Error fetching users:', error);
    });
  }

  onUserClick(userId: number) {
    this.selectedUserId = userId;
    console.log('Selected User ID:', this.selectedUserId);
  }

  deleteUser() {
    if (this.selectedUserId != null && this.selectedUserId != 1) {
      this.userService.deleteUser(this.selectedUserId).subscribe(() => {
        this.loadUsers();
        this.selectedUserId = null;
      }, error => {
        console.error('Error deleting user:', error);
      });
    }
  }
}

