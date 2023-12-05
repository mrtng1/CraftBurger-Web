import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {CartService} from "../../service/cart.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit{
  @Output() aboutUsClick = new EventEmitter<void>();
  cartCount: number = 0;

  constructor(private router: Router, private route: ActivatedRoute, private cartService: CartService) {}

  ngOnInit() {
    console.log('Header Component: ngOnInit');

    setTimeout(() => {
      this.cartService.cartCount$.subscribe(count => {
        console.log('Header Component: cartCount$ subscription callback', count);
        this.cartCount = count;
      });
    }, 0);

    const cart = sessionStorage.getItem('cart');
    this.cartCount = cart ? JSON.parse(cart).length : 0;
  }


  aboutUsClicked() {
    if (this.route.snapshot.routeConfig?.path === 'home') {
      this.aboutUsClick.emit();
    } else {
      this.router.navigate(['/home']).then(() => {
        this.aboutUsClick.emit();
      });
    }
  }
}
