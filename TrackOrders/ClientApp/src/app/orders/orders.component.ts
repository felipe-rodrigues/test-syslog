import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  orders: any;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router) { }

  ngOnInit(): void {
    this.loadOrders();
  }


  saveAttempt(orderNumber: string, delivered: boolean){
    this.http.put<any>(`${this.baseUrl}api/order/${orderNumber}/delivery?delivered=${delivered.toString()}`,{}).subscribe(data => {
      this.loadOrders();
    })
  }

  loadOrders() {
    this.http.get<any>(`${this.baseUrl}api/order`).subscribe(data => {
      this.orders = data;
    });
  }

  add() {
    this.router.navigate(['add-order']);
  }

}
