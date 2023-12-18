import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, NavigationEnd, Router} from "@angular/router";
import {filter} from 'rxjs/operators';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  showSubMenu: boolean = false;
  selectedNavItem: string = 'Overview';

  // Map route paths to display names
  routeDisplayNameMap: { [key: string]: string } = {
    'user-management': 'User Management',
    'item-management': 'Item Management',
    'overview': 'Overview',
    // Add more mappings as needed
  };

  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
  }

  ngOnInit() {
    // Subscribe to router events to detect navigation changes
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      // Set the selectedNavItem based on the route
      this.setSelectedNavItem(this.activatedRoute);
    });
  }

  setSelectedNavItem(route: ActivatedRoute): void {
    while (route.firstChild) {
      route = route.firstChild;
    }

    const routePath = route.routeConfig?.path;
    this.selectedNavItem = routePath ? this.routeDisplayNameMap[routePath] || 'Management' : 'Management';
    this.showSubMenu = false; // Hide submenu after navigation
  }

  toggleSubMenu() {
    this.showSubMenu = !this.showSubMenu;
  }
}
