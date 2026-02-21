import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LeaderBoardRequest, UserAddFieldRequest, AddPointRequest } from '../models/shared.model';
import { BaseGetAllRequest } from '../models/post.model';

@Injectable({ providedIn: 'root' })
export class LevelService {
  private base = `${environment.apiUrl}/level`;

  constructor(private http: HttpClient) {}

  getAllLevels(req?: BaseGetAllRequest): Observable<any> {
    return this.http.get(`${this.base}/level/all`, { params: req as any });
  }

  getLevelById(id: string): Observable<any> {
    return this.http.get(`${this.base}/level/${id}`);
  }

  addFieldsForUser(req: UserAddFieldRequest): Observable<any> {
    return this.http.post(`${this.base}/user/field/add`, req);
  }

  getFieldsOfUser(userId: string): Observable<any> {
    return this.http.get(`${this.base}/user/all/${userId}`);
  }

  addPoint(req: AddPointRequest): Observable<any> {
    return this.http.post(`${this.base}/add-point`, req);
  }

  getLevelByObject(objectId: string): Observable<any> {
    return this.http.get(`${this.base}/objectlevel/${objectId}`);
  }

  getLeaderBoard(req: LeaderBoardRequest): Observable<any> {
    return this.http.get(`${this.base}/leader-board`, { params: req as any });
  }
}
