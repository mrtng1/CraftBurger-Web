import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {InspectItemComponent} from "./inspect-item/inspect-item.component";
import {MainComponent} from "./main/main.component";
import {MenuComponent} from "./menu/menu.component";
import {CartComponent} from "./cart/cart.component";
import {LoginComponent} from "./login/login.component";
import {AdminComponent} from "./admin/admin.component";
import {AuthGuard} from "./auth.guard";

const routes: Routes = [
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
  {
    path: 'home',
    component: MainComponent,
    data: { showHeader: true }
  },
  {
    path: 'inspect/:burgerId',
    component: InspectItemComponent,
    data: { showHeader: true }
  },
  {
    path: 'menu',
    component: MenuComponent,
    data: { showHeader: true }
  },
  {
    path: 'cart',
    component: CartComponent,
    data: { showHeader: true }
  },
  {
    path: 'login',
    component: LoginComponent,
    data: { showHeader: false }
  },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [AuthGuard],
    data: { showHeader: false }
  },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
