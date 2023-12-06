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
    const userDetails = this.getUserDetailsFromToken();
    if (!userDetails.email) {
      console.error('No user email found in token');
      return;
    }

    const subject = 'Din ordrebekræftelse';
    let body = `<html><body style="background-color: #1b1b1b; color: white;">`;
    body += `<p>Hej ${userDetails.name || 'Kunde'},</p>`;
    body += `<p>Tak for din ordre. Her er detaljerne:</p>`;

    this.cartItems.forEach(item => {
      body += `<div style="margin-bottom: 10px;">`;
      if(item.imageUrl) {
        body += `<img src="${item.imageUrl}" alt="${item.name}" style="width: 100px; height: auto; float: left; margin-right: 10px;">`;
      }
      body += `<p>${item.name}<br>Mængde: ${item.quantity}<br>Pris: ${item.price} kr</p>`;
      body += `</div><div style="clear: both;"></div>`;
    });

    const formattedDate = new Date().toLocaleDateString('da-DK', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
    });

    body += `<p>Total Pris: ${this.totalPrice} kr</p>`;
    body += `<p>Ordredato: ${formattedDate}</p>`;
    body += `<p>Vi ser frem til at se dig igen!</p>`;
    body += `</body></html>`;

    this.mailService.sendEmail(userDetails.email, subject, body).subscribe({
      next: (response) => console.log('Email sent successfully', response),
      error: (error) => console.error('Error sending email', error)
    });
  }


  private getUserDetailsFromToken(): { email: string | null, name: string | null } {
    const token = localStorage.getItem('SessionToken');
    if (!token) {
      return { email: null, name: null };
    }

    try {
      const payload = token.split('.')[1];
      const decodedJson = atob(payload);
      const decodedToken = JSON.parse(decodedJson);
      const email = decodedToken.email;
      const name = decodedToken.unique_name || decodedToken.name; // Use the appropriate key here
      return { email, name };
    } catch (e) {
      console.error('Error decoding token', e);
      return { email: null, name: null };
    }
  }
}
