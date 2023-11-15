import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {InspectItemComponent} from "./inspect-item/inspect-item.component";
import {MainComponent} from "./main/main.component";
import {MenuComponent} from "./menu/menu.component";
import {CartComponent} from "./cart/cart.component";
import {LoginComponent} from "./login/login.component";

const routes: Routes = [
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
  {
    path: 'home',
    component: MainComponent // The main page
  },
  {
    path: 'inspect/:burgerId',
    component: InspectItemComponent
  },
  {
    path: 'menu',
    component: MenuComponent
  },
  {
    path: 'cart',
    component: CartComponent
  },
  {
    path: 'login',
    component: LoginComponent
  }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
