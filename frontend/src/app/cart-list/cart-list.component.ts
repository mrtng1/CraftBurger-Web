import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-cart-list',
  templateUrl: './cart-list.component.html',
  styleUrls: ['./cart-list.component.css']
})
export class CartListComponent implements OnInit {
  cartItems: any[] = [];

  ngOnInit() {
    this.loadCartItems();
  }

  loadCartItems() {
    let cart = sessionStorage.getItem('cart');
    if (cart) {
      this.cartItems = JSON.parse(cart);
      console.log(this.cartItems); // Debugging: log the cart items
    }
  }
}
