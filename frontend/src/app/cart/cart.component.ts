import { Component, OnInit, ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  cartItems: any[] = [];

  constructor(private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.loadCartItems();
  }

  loadCartItems() {
    let cart = sessionStorage.getItem('cart');
    if (cart) {
      this.cartItems = JSON.parse(cart);
      this.updateTotalPrice();
    }
  }

  get totalPrice(): number {
    const calculatedTotal = this.cartItems.reduce((sum, item) => {
      const isBurger = item.burgerPrice !== undefined;
      const isFries = item.friesPrice !== undefined;

      if (isBurger) {
        sum += item.burgerPrice * item.quantity;
      } else if (isFries) {
        sum += item.friesPrice * item.quantity;
      }

      return sum;
    }, 0);

    console.log('Calculated Total:', calculatedTotal);

    return calculatedTotal;
  }

  updateTotalPrice() {
    this.cdr.detectChanges();
    console.log('Updated Total Price:', this.totalPrice);
  }

  removeItem(item: any) {
    // Remove the item from the cartItems array
    this.cartItems = this.cartItems.filter((cartItem) => cartItem !== item);

    // Update the sessionStorage with the new cartItems
    sessionStorage.setItem('cart', JSON.stringify(this.cartItems));

    // Manually trigger change detection to update the totalPrice
    this.updateTotalPrice();
  }
}
