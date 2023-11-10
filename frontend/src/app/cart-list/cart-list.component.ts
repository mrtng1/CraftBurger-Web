import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-cart-list',
  templateUrl: './cart-list.component.html',
  styleUrls: ['./cart-list.component.css']
})
export class CartListComponent implements OnInit {
  cartItems: string[] = [];

  ngOnInit() {
    this.loadCartItems();
  }

  loadCartItems() {
    const cart = sessionStorage.getItem('cart');
    if (cart) {
      this.cartItems = JSON.parse(cart);
    }
  }
}
