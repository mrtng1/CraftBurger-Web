import {Component, OnInit} from '@angular/core';
import {CommonModule} from '@angular/common';
import {OrderService} from "../../../service/order.service";
import {FriesService} from "../../../service/fries.service";
import {BurgerService} from "../../../service/burger.service";
import {UserService} from "../../../service/user.service";
import {DipService} from "../../../service/dip.service";

@Component({
  selector: 'app-overview',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.css']
})
export class OverviewComponent implements OnInit {
  orders: any[] = [];
  orderDetails: any[] = [];
  mostCommonOrderHour: string | undefined;
  mostPreferredItemType: string | undefined;
  averageOrderPrice: number | undefined;

  selectedOrderDetails: any[] = [];
  showOrderDetails: boolean = false;

  constructor(private orderService: OrderService,
              private burgerService: BurgerService,
              private friesService: FriesService,
              private dip: DipService,
              private userService: UserService
  ) {
  }

  ngOnInit() {
    this.loadOrders();
    this.loadOrderDetails();
  }

  loadOrders() {
    this.orderService.getAllUserOrders().subscribe(data => {
      this.orders = data;
      this.orders.forEach(order => {
        this.userService.getUserById(order.userId).subscribe(user => {
          order.userName = user.username;
        });
      });
      this.calculateOrderTimeStatistics();
      this.calculateAverageOrderPrice();
    }, error => {
      console.error('Error fetching orders:', error);
    });
  }

  loadOrderDetails() {
    this.orderService.getAllOrderDetails().subscribe(data => {
      this.orderDetails = data;
      this.calculateItemTypeStatistics();
    }, error => {
      console.error('Error fetching order details:', error);
    });
  }

  calculateOrderTimeStatistics() {
    const hourCounts = this.orders.reduce((acc, order) => {
      const hour = new Date(order.orderDate).getHours();
      acc[hour] = (acc[hour] || 0) + 1;
      return acc;
    }, {});

    const mostCommonHour = Object.keys(hourCounts).reduce((a, b) => hourCounts[a] > hourCounts[b] ? a : b);
    this.mostCommonOrderHour = `${mostCommonHour}:00`;
  }

  calculateItemTypeStatistics() {
    const itemTypeCounts = this.orderDetails.reduce((acc, detail) => {
      const itemType = detail.itemType;
      acc[itemType] = (acc[itemType] || 0) + 1;
      return acc;
    }, {});

    const mostCommonItemType = Object.keys(itemTypeCounts).reduce((a, b) => itemTypeCounts[a] > itemTypeCounts[b] ? a : b);
    this.mostPreferredItemType = mostCommonItemType;
  }

  calculateAverageOrderPrice() {
    if (this.orders.length > 0) {
      const totalSum = this.orders.reduce((sum, order) => sum + order.totalPrice, 0);
      this.averageOrderPrice = totalSum / this.orders.length;
    }
  }

  onOrderClick(orderId: number) {
    this.showOrderDetails = true;
    this.selectedOrderDetails = [];

    const details = this.orderDetails.filter(detail => detail.orderId === orderId);

    details.forEach(detail => {
      if (detail.itemType === 'burger') {
        this.burgerService.getBurgerById(detail.itemId).subscribe(burger => {
          this.selectedOrderDetails.push({...burger, quantity: detail.quantity});
        });
      } else if (detail.itemType === 'fries') {
        this.friesService.getFriesById(detail.itemId).subscribe(fries => {
          this.selectedOrderDetails.push({...fries, quantity: detail.quantity});
        });
      } else if (detail.itemType === 'dip') {
        this.dip.getDipById(detail.itemId).subscribe((dip: any) => {
          this.selectedOrderDetails.push({...dip, quantity: detail.quantity});
        });
      }
    });
  }
}
