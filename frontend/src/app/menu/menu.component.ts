import {Component, OnInit} from '@angular/core';
import {BurgerService} from '../../service/burger.service';
import {FriesService} from '../../service/fries.service';
import {CartItem} from '../../models/CartItem';
import {ImageService} from "../../service/image.service";
import {CartService} from "../../service/cart.service";
import {DipService} from "../../service/dip.service";

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  burgers: any[] = [];
  fries: any[] = [];
  dips: any[] = [];
  selectedCategory: string = 'All';

  constructor(private burgerService: BurgerService, public friesService: FriesService, public imageService: ImageService, private cartService: CartService,
              private dipService: DipService) {
  }

  ngOnInit() {
    this.loadBurgers();
    this.loadFries();
    this.loadDips();
  }

  loadBurgers() {
    this.burgerService.getBurgers().subscribe({
      next: (data) => {
        this.burgers = data;
      },
      error: (error) => console.error('Error fetching burgers:', error)
    });
  }

  loadFries() {
    this.friesService.getFries().subscribe({
      next: (data) => {
        this.fries = data;
      },
      error: (error) => console.error('Error fetching fries:', error)
    });
  }

  loadDips() {
    this.dipService.getDips().subscribe({
      next: (data) => {
        this.dips = data;
      },
      error: (error) => console.error('Error fetching dips:', error)
    });
  }

  addToCart(item: CartItem, itemType: string) {
    let cart = sessionStorage.getItem('cart');
    let cartArray: CartItem[] = cart ? JSON.parse(cart) : [];

    const newItem = {
      ...item,
      quantity: 1,
      itemType: itemType
    };

    const existingItemIndex = cartArray.findIndex((cartItem: CartItem) => cartItem.id === item.id && cartItem.itemType === itemType);

    if (existingItemIndex !== -1) {
      cartArray[existingItemIndex].quantity = (cartArray[existingItemIndex].quantity || 0) + 1;
    } else {
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
