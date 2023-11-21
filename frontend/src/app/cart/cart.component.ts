import { Component, OnInit, ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  cartItems: any[] = [];

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
      const itemPrice = item.burgerPrice || item.friesPrice || 0;
      const quantity = item.quantity || 0;

      return sum + itemPrice * quantity;
    }, 0);
  }

  removeItem(item: any) {
    this.cartItems = this.cartItems.filter((cartItem) => cartItem !== item);
    sessionStorage.setItem('cart', JSON.stringify(this.cartItems));
  }
}
