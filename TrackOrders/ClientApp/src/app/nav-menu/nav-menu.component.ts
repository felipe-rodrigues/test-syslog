import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../shared/services/auth.service';
import { User } from '../shared/models/user';
import { HttpClient } from '@angular/common/http';
import { SignalRService } from '../shared/services/signalr.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit  {
  isExpanded = false;
  currentUser: Observable<User | null>;
  unreadNotifications : any;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  constructor (private auth: AuthService, private router: Router, private http:HttpClient , @Inject('BASE_URL') private baseUrl: string, private signalRService: SignalRService){
    this.currentUser = this.auth.currentUser;
    this.loadUnreadNotifications();
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['']);
  }

  ngOnInit(): void {
    this.signalRService.message$.subscribe((mensagemRecebida) => {
      if (mensagemRecebida) {
        this.loadUnreadNotifications();
      }
    });
  }

  loadUnreadNotifications(){
    this.http.get<any[]>(`${this.baseUrl}api/log`).subscribe(data => {
      var total = data.length;
      var viewed = data.filter(el => el.viewed == true).length;
      this.unreadNotifications = total - viewed;
    })
  }
}
