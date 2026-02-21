import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AddUserRequest, UpdateUserRequest, AddAvatarRequest,
  AddFollowerRequest, FilterUserRequest, FollowFilterRequest
} from '../models/user.model';
import { BaseGetAllRequest } from '../models/post.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private base = `${environment.apiUrl}/user`;

  constructor(private http: HttpClient) {}

  register(req: AddUserRequest): Observable<any> {
    return this.http.post(`${this.base}/register`, req);
  }

  updateUser(req: UpdateUserRequest): Observable<any> {
    return this.http.put(`${this.base}/update`, req);
  }

  updateAddress(req: any): Observable<any> {
    return this.http.post(`${this.base}/update-address`, req);
  }

  uploadAvatar(req: AddAvatarRequest): Observable<any> {
    const body = {
      avatar_hash: req.avatarHash,
      description: req.description ?? ''
    };
    return this.http.post(`${this.base}/avatar`, body);
  }

  updateAvatar(req: AddAvatarRequest): Observable<any> {
    const body = {
      avatar_hash: req.avatarHash,
      description: req.description ?? ''
    };
    return this.http.post(`${this.base}/avatar/update`, body);
  }

  follow(req: AddFollowerRequest): Observable<any> {
    return this.http.post(`${this.base}/following`, req);
  }

  unfollow(followingId: string): Observable<any> {
    return this.http.post(`${this.base}/following/remove?followingId=${followingId}`, {});
  }

  getUserById(id: string): Observable<any> {
    return this.http.get(`${this.base}/get/${id}`);
  }

  getCurrentUser(): Observable<any> {
    return this.http.get(`${this.base}/current`);
  }

  getFollowers(req: FollowFilterRequest): Observable<any> {
    return this.http.post(`${this.base}/get-follower`, req);
  }

  getFollowing(req: FollowFilterRequest): Observable<any> {
    return this.http.post(`${this.base}/get-following`, req);
  }

  filterUsers(req: FilterUserRequest): Observable<any> {
    return this.http.post(`${this.base}/user/filter`, req);
  }

  getNearbyUsers(req: BaseGetAllRequest): Observable<any> {
    return this.http.get(`${this.base}/near-by`, { params: req as any });
  }

  isEmailExist(email: string): Observable<any> {
    return this.http.get(`${this.base}/is-exist`, { params: { email } });
  }

  addAdditionalInfos(infos: any[]): Observable<any> {
    return this.http.post(`${this.base}/additionalinfos`, infos);
  }

  turnOnNotification(objectId: string): Observable<any> {
    return this.http.post(`${this.base}/add-notification/${objectId}`, {});
  }

  turnOffNotification(objectId: string): Observable<any> {
    return this.http.post(`${this.base}/remove-notification/${objectId}`, {});
  }

  getHistory(req: BaseGetAllRequest): Observable<any> {
    return this.http.get(`${this.base}/history`, { params: req as any });
  }
}
