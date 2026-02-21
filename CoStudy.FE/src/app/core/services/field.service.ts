import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  Field,
  FieldGroup,
  AddFieldRequest,
  AddFieldGroupRequest,
  AddFieldToGroupRequest,
  FieldApiResponse,
  FieldPaginationRequest
} from '../models/field.model';

@Injectable({ providedIn: 'root' })
export class FieldService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/field`;
  private groupBase = `${environment.apiUrl}/fieldgroup`;

  private buildParams(req?: FieldPaginationRequest): HttpParams {
    let params = new HttpParams();
    if (req?.skip != null) params = params.set('skip', req.skip.toString());
    if (req?.count != null) params = params.set('count', req.count.toString());
    return params;
  }

  getAllFields(req?: FieldPaginationRequest): Observable<FieldApiResponse<Field[]>> {
    return this.http.get<FieldApiResponse<Field[]>>(this.base, { params: this.buildParams(req) });
  }

  getFieldByGroupId(groupId: string): Observable<FieldApiResponse<Field[]>> {
    return this.http.get<FieldApiResponse<Field[]>>(`${this.base}/group-id/${groupId}`);
  }

  addField(field: AddFieldRequest): Observable<FieldApiResponse<Field>> {
    return this.http.post<FieldApiResponse<Field>>(this.base, field);
  }

  deleteField(id: string): Observable<FieldApiResponse<string>> {
    return this.http.delete<FieldApiResponse<string>>(`${this.base}/${id}`);
  }

  getAllFieldGroups(req?: FieldPaginationRequest): Observable<FieldApiResponse<FieldGroup[]>> {
    return this.http.get<FieldApiResponse<FieldGroup[]>>(`${this.groupBase}/field-group`, { params: this.buildParams(req) });
  }

  addFieldGroup(group: AddFieldGroupRequest): Observable<FieldApiResponse<FieldGroup>> {
    return this.http.post<FieldApiResponse<FieldGroup>>(this.groupBase, group);
  }

  addFieldToGroup(req: AddFieldToGroupRequest): Observable<FieldApiResponse<FieldGroup>> {
    return this.http.post<FieldApiResponse<FieldGroup>>(`${this.groupBase}/add-field`, req);
  }

  removeFieldFromGroup(req: AddFieldToGroupRequest): Observable<FieldApiResponse<FieldGroup>> {
    return this.http.post<FieldApiResponse<FieldGroup>>(`${this.groupBase}/remove-field`, req);
  }
}
