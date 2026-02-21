import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AddPostRequest, UpdatePostRequest, GetPostByUserRequest,
  NewsFeedRequest, GetSavedPostRequest, FilterRequest,
  SharePostRequest, ModifedPostStatusRequest, BaseGetAllRequest
} from '../models/post.model';

@Injectable({ providedIn: 'root' })
export class PostService {
  private base = `${environment.apiUrl}/post`;

  constructor(private http: HttpClient) {}

  addPost(req: AddPostRequest): Observable<any> {
    return this.http.post(this.base, req);
  }

  getById(id: string): Observable<any> {
    return this.http.get(`${this.base}/${id}`);
  }

  getByUserId(req: GetPostByUserRequest): Observable<any> {
    return this.http.post(`${this.base}/user`, req);
  }

  getTimeline(req: BaseGetAllRequest): Observable<any> {
    return this.http.post(`${this.base}/timeline`, req);
  }

  getNewsFeed(req: NewsFeedRequest): Observable<any> {
    return this.http.post(`${this.base}/news-feed`, req);
  }

  upvote(id: string): Observable<any> {
    return this.http.post(`${this.base}/upvote/${id}`, {});
  }

  downvote(id: string): Observable<any> {
    return this.http.post(`${this.base}/downvote/${id}`, {});
  }

  updatePost(req: UpdatePostRequest): Observable<any> {
    return this.http.put(`${this.base}/update`, req);
  }

  savePost(id: string): Observable<any> {
    return this.http.post(`${this.base}/save/${id}`, {});
  }

  getSavedPosts(req: GetSavedPostRequest): Observable<any> {
    return this.http.get(`${this.base}/save`, { params: req as any });
  }

  filterPosts(req: FilterRequest): Observable<any> {
    return this.http.post(`${this.base}/filter`, req);
  }

  sharePost(req: SharePostRequest): Observable<any> {
    return this.http.post(`${this.base}/share-post`, req);
  }

  modifiedStatus(req: ModifedPostStatusRequest): Observable<any> {
    return this.http.post(`${this.base}/modified-post-status`, req);
  }

  getUserMatchPost(postId: string): Observable<any> {
    return this.http.get(`${this.base}/match-user`, { params: { postId } });
  }

  getHistory(req: BaseGetAllRequest): Observable<any> {
    return this.http.get(`${this.base}/history`, { params: req as any });
  }
}
