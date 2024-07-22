import { Inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HttpTransportType } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;
  private messageSubject = new BehaviorSubject<{ usuario: string, mensagem: string } | any>(null);

  message$ = this.messageSubject.asObservable();

  constructor(@Inject('BASE_URL') private baseUrl: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`https://localhost:7225/notificationHub`,{
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection.on('OrderEvents', (usuario, mensagem) => {
      this.messageSubject.next({ mensagem });
    });

    this.hubConnection.start()
      .catch(err => console.error('Erro ao conectar ao Hub SignalR: ', err));
  }
}
