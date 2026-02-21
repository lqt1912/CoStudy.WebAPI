import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { BaseGetAllRequest } from '../models/post.model';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private base = `${environment.apiUrl}/noftication`;

  constructor(private http: HttpClient) {}

  getNotifications(req: BaseGetAllRequest): Observable<any> {
    return this.http.get(`${this.base}/current`, { params: req as any });
  }

  markAsRead(id: string): Observable<any> {
    return this.http.put(`${this.base}/read?id=${id}`, {});
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.base}/${id}`);
  }

  deleteAll(): Observable<any> {
    return this.http.delete(`${this.base}/delete-all`);
  }
}
