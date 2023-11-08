import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../Environments/environment";

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  burgers: any[] = []; // Replace 'any' with an interface or class for the burger data

  constructor(private http: HttpClient) {
  }

  ngOnInit() {
    this.http.get<any[]>(`${environment.baseUrl}/api/burgers`).subscribe({
      next: (data) => {
        this.burgers = data;
      },
      error: (error) => {
        console.error('Error fetching burgers:', error);
      }
    });
  }
}
