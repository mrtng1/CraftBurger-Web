import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderService } from "../../../service/order.service";

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

  constructor(private orderService: OrderService) {}

  ngOnInit() {
    this.loadOrders();
    this.loadOrderDetails();
  }

  loadOrders() {
    this.orderService.getAllUserOrders().subscribe(data => {
      this.orders = data;
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
}
