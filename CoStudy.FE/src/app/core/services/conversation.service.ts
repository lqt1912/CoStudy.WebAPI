import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AddConversationRequest, AddMemberRequest } from '../models/message.model';

@Injectable({ providedIn: 'root' })
export class ConversationService {
  private base = `${environment.apiUrl}/conversation`;

  constructor(private http: HttpClient) {}

  addConversation(req: AddConversationRequest): Observable<any> {
    return this.http.post(this.base, req);
  }

  getCurrentConversations(): Observable<any> {
    return this.http.get(`${this.base}/current`);
  }

  deleteConversation(id: string): Observable<any> {
    return this.http.delete(`${this.base}/${id}`);
  }

  addMember(req: AddMemberRequest): Observable<any> {
    return this.http.put(`${this.base}/member`, req);
  }
}
