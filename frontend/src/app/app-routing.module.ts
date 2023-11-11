import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {InspectItemComponent} from "./inspect-item/inspect-item.component";
import {MainComponent} from "./main/main.component";
import {AppComponent} from "./app.component";

const routes: Routes = [
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full' // This ensures that the redirect only occurs for the exact path ''
  },
  {
    path: 'home',
    component: MainComponent // The main page
  },
  {
    path: 'inspect/:burgerId',
    component: InspectItemComponent // The inspect page
  }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
