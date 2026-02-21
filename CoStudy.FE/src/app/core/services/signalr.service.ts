import { Injectable, OnDestroy } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';
import { Message } from '../models/message.model';

@Injectable({ providedIn: 'root' })
export class SignalRService implements OnDestroy {
  private hubConnection: signalR.HubConnection | null = null;

  /** Emits new incoming messages */
  private messageSubject = new Subject<Message>();
  message$ = this.messageSubject.asObservable();

  /** Connection state */
  private connectedSubject = new BehaviorSubject<boolean>(false);
  connected$ = this.connectedSubject.asObservable();

  constructor(private authService: AuthService) {}

  startConnection(): void {
    if (this.hubConnection) return;

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.signalRHubUrl}/chat`, {
        accessTokenFactory: () => this.authService.token ?? ''
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Warning)
      .build();

    this.hubConnection
      .start()
      .then(() => {
        this.connectedSubject.next(true);
        this.registerHandlers();
      })
      .catch(err => console.error('SignalR connection error:', err));

    this.hubConnection.onreconnected(() => this.connectedSubject.next(true));
    this.hubConnection.onclose(() => this.connectedSubject.next(false));
  }

  private registerHandlers(): void {
    this.hubConnection?.on('ReceiveMessage', (message: Message) => {
      this.messageSubject.next(message);
    });
  }

  stopConnection(): void {
    this.hubConnection?.stop();
    this.hubConnection = null;
    this.connectedSubject.next(false);
  }

  ngOnDestroy(): void {
    this.stopConnection();
  }
}
