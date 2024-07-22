import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {
  notifications: any;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit(): void {
   this.loadNotifications();
  }

  saveViewedNotification(notification: any){
    this.http.post<any>(`${this.baseUrl}api/log/order/${notification.orderNumber}/viewed`,{
      orderNumber: notification.orderNumber,
      message: notification.message,
      attempt: notification.attempt,
      hasDelivered: notification.hasDelivered,
      isNewOrderNotification: notification.isNewOrderNotification
    }).subscribe(data => {
      console.log('visualização salva');
      this.loadNotifications();
    },err => alert('Erro ao salvar visualização'));
  }

  loadNotifications(){
    this.http.get<any>(`${this.baseUrl}api/log`).subscribe(data => {
      this.notifications = data;
    });
  }

}
