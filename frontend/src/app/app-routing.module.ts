import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {InspectItemComponent} from "./inspect-item/inspect-item.component";
import {MainComponent} from "./main/main.component";
import {MenuComponent} from "./menu/menu.component";
import {CartComponent} from "./cart/cart.component";
import {LoginComponent} from "./login/login.component";
import {AdminComponent} from "./admin/admin.component";
import {AuthGuard} from "./auth.guard";
import {ItemManagementComponent} from "./admin/item-management/item-management.component";
import {UserManagementComponent} from "./admin/user-management/user-management.component";
import {OverviewComponent} from "./admin/overview/overview.component";

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
    data: { showHeader: false },
    children: [ {
      path: 'item-management',
      component: ItemManagementComponent,
    },
      {
        path: 'user-management',
        component: UserManagementComponent,
      },
      {
        path: 'overview',
        component: OverviewComponent,
      },
    ]
  },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
