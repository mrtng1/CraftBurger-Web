import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {InspectItemComponent} from "./inspect-item/inspect-item.component";
import {MainComponent} from "./main/main.component";
import {MenuComponent} from "./menu/menu.component";

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
    component: InspectItemComponent // The inspect page
  },
  {
    path: 'menu',
    component: MenuComponent // The menu page
  }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
