import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../Environments/environment";


@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  burgers: any[] = [];
  currentIndex: number = 0;
  imageExtensions: { [key: string]: string } = {
    'Cheeseburger': 'jpg',
    'Truffleburger': 'png',
    'hot-bird': 'JPG',
    'monster-burger': 'JPG',
    'kids-burger': 'JPG',
    'bernaise-burger': 'JPG',
    'cherry-burger': 'JPG',
  };

  constructor(private http: HttpClient) {
  }

  ngOnInit() {
    this.http.get<any[]>(`${environment.baseUrl}/api/burgers`).subscribe({
      next: (data) => {
        this.burgers = data.map(burger => ({
          ...burger,
          imageUrl: this.getImageUrl(burger.burgerName)
        }));
      },
      error: (error) => {
        console.error('Error fetching burgers:', error);
      }
    });
  }

  getImageUrl(burgerName: string): string {
    const formattedName = burgerName.toLowerCase().replace(/\s+/g, '-');
    const extension = this.imageExtensions[burgerName] || 'JPG';
    return `/assets/burgers/${formattedName}.${extension}`;
  }

  nextBurger() {
    this.currentIndex = (this.currentIndex + 1) % this.burgers.length;
  }

  previousBurger() {
    this.currentIndex =
      (this.currentIndex - 1 + this.burgers.length) % this.burgers.length;
  }


}
