import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AddMessageRequest, UpdateMessageRequest, GetMessageByConversationIdRequest } from '../models/message.model';

@Injectable({ providedIn: 'root' })
export class MessageService {
  private base = `${environment.apiUrl}/message`;

  constructor(private http: HttpClient) {}

  addMessage(req: AddMessageRequest): Observable<any> {
    return this.http.post(`${this.base}/message/add`, req);
  }

  getByConversationId(req: GetMessageByConversationIdRequest): Observable<any> {
    return this.http.get(`${this.base}/message/get/conversation`, { params: req as any });
  }

  deleteMessage(id: string): Observable<any> {
    return this.http.delete(`${this.base}/message/${id}`);
  }

  editMessage(req: UpdateMessageRequest): Observable<any> {
    return this.http.put(`${this.base}/update`, req);
  }
}
