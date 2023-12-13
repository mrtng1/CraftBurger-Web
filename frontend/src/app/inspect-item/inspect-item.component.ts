import {Component, OnInit} from '@angular/core';
import {environment} from "../../Environments/environment";
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-inspect-item',
  templateUrl: './inspect-item.component.html',
  styleUrls: ['./inspect-item.component.css']
})
export class InspectItemComponent implements OnInit {
  burger: any;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const burgerId = params.get('burgerId');
      if (burgerId) {
        this.http.get<any>(`${environment.baseUrl}/api/burger/${burgerId}`).subscribe(data => {
          this.burger = data;
        });
      }
    });
  }


  getImageUrl(burgerName: string): string {
    const formattedName = burgerName.toLowerCase().replace(/\s+/g, '-');
    return `/assets/burgers/${formattedName}.JPG`; // Directly use .JPG extension
  }
}
