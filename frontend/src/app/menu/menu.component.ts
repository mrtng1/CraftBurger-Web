import { Component, OnInit } from '@angular/core';
import { BurgerService } from '../service/burger.service';
import { FriesService } from '../service/fries.service';
import { CartItem } from '../models/CartItem';
import { ImageService } from "../service/image.service";
import { CartService } from "../service/cart.service";

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  burgers: any[] = [];
  fries: any[] = [];
  selectedCategory: string = 'All';

  constructor(private burgerService: BurgerService, public friesService: FriesService, public imageService: ImageService, private cartService: CartService) {}

  ngOnInit() {
    this.loadBurgers();
    this.loadFries();
  }

  loadBurgers() {
    this.burgerService.getBurgers().subscribe({
      next: (data) => {
        this.burgers = data.map(burger => ({
          ...burger,
          imageUrl: this.imageService.getImageUrl(burger.burgerName, "burger")
        }));
      },
      error: (error) => console.error('Error fetching burgers:', error)
    });
  }

  loadFries() {
    this.friesService.getFries().subscribe({
      next: (data) => {
        this.fries = data.map(fry => ({
          ...fry,
          imageUrl: this.imageService.getImageUrl(fry.friesName, "fries")
        }));
      },
      error: (error) => console.error('Error fetching fries:', error)
    });
  }


  addToCart(item: CartItem, itemType: string) {
    let cart = sessionStorage.getItem('cart');
    let cartArray: CartItem[] = cart ? JSON.parse(cart) : [];

    const existingItemIndex = cartArray.findIndex((cartItem: CartItem) => cartItem.id === item.id);

    if (existingItemIndex !== -1) {
      cartArray[existingItemIndex].quantity = (cartArray[existingItemIndex].quantity || 0) + 1;
    } else {
      const newItem = { ...item, quantity: 1 };
      cartArray.push(newItem);
    }

    sessionStorage.setItem('cart', JSON.stringify(cartArray));
    this.cartService.updateCartCount(cartArray.length);
  }

  getImageUrl(burgerName: string): string {
    return this.imageService.getImageUrl(burgerName, "burger");
  }

  updateSelectedCategory(category: string): void {
    this.selectedCategory = category;
  }

  shouldDisplayItem(itemType: string): boolean {
    return this.selectedCategory === 'All' || this.selectedCategory === itemType;
  }
}
