import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { BaseGetAllRequest } from '../models/post.model';

@Injectable({ providedIn: 'root' })
export class FieldService {
  private base = `${environment.apiUrl}/field`;
  private groupBase = `${environment.apiUrl}/fieldgroup`;

  constructor(private http: HttpClient) {}

  getAllFields(req?: BaseGetAllRequest): Observable<any> {
    return this.http.get(this.base, { params: req as any });
  }

  getFieldByGroupId(groupId: string): Observable<any> {
    return this.http.get(`${this.base}/group-id/${groupId}`);
  }

  addField(field: any): Observable<any> {
    return this.http.post(this.base, field);
  }

  deleteField(id: string): Observable<any> {
    return this.http.delete(`${this.base}/${id}`);
  }

  getAllFieldGroups(req?: BaseGetAllRequest): Observable<any> {
    return this.http.get(`${this.groupBase}/field-group`, { params: req as any });
  }

  addFieldGroup(group: any): Observable<any> {
    return this.http.post(this.groupBase, group);
  }

  addFieldToGroup(req: any): Observable<any> {
    return this.http.post(`${this.groupBase}/add-field`, req);
  }

  removeFieldFromGroup(req: any): Observable<any> {
    return this.http.post(`${this.groupBase}/remove-field`, req);
  }
}
