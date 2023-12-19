import {Injectable} from '@angular/core';
import {CartService} from './cart.service';

@Injectable({
  providedIn: 'root',
})
export class InitializationService {
  constructor(private cartService: CartService) {
  }

  initializeCartCount() {
    const cart = sessionStorage.getItem('cart');
    const initialCount = cart ? JSON.parse(cart).length : 0;
    this.cartService.updateCartCount(initialCount);
  }
}
