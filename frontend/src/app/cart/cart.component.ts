import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import {CartService} from "../../service/cart.service";
import {MailService} from "../../service/mail.service";

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  cartItems: any[] = [];

  constructor(private cartService: CartService, private mailService: MailService) {}

  ngOnInit() {
    this.loadCartItems();
  }

  loadCartItems() {
    let cart = sessionStorage.getItem('cart');
    if (cart) {
      this.cartItems = JSON.parse(cart);
      this.sortCartItems();
    }
  }

  sortCartItems() {
    // Custom sorting function: burgers first, then fries
    this.cartItems.sort((a, b) => {
      const typeA = a.burgerName ? 'burger' : 'fries';
      const typeB = b.burgerName ? 'burger' : 'fries';

      if (typeA < typeB) return -1;
      if (typeA > typeB) return 1;
      return 0;
    });
  }

  get totalPrice(): number {
    return this.cartItems.reduce((sum, item) => {
      const itemPrice = item.price || 0;
      const quantity = item.quantity || 0;

      return sum + itemPrice * quantity;
    }, 0);
  }

  removeItem(item: any) {
    this.cartItems = this.cartItems.filter((cartItem) => cartItem !== item);
    sessionStorage.setItem('cart', JSON.stringify(this.cartItems));
    this.cartService.updateCartCount(this.cartItems.length);
  }

  completeOrder() {
    const userEmail = this.getUserEmailFromToken();
    if (!userEmail) {
      console.error('No user email found in token');
      return;
    }
    const subject = 'Your Order Confirmation';
    const body = `Your order total is: ${this.totalPrice}`;

    this.mailService.sendEmail(userEmail, subject, body).subscribe({
      next: (response) => console.log('Email sent successfully', response),
      error: (error) => console.error('Error sending email', error)
    });
  }

  private getUserEmailFromToken(): string | null {
    const token = localStorage.getItem('SessionToken');
    if (!token) {
      return null;
    }

    try {
      const payload = token.split('.')[1];
      const decodedJson = atob(payload);
      const decodedToken = JSON.parse(decodedJson);
      return decodedToken.email;
    } catch (e) {
      console.error('Error decoding token', e);
      return null;
    }
  }
}
