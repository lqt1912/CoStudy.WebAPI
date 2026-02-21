import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AddCommentRequest, ReplyCommentRequest, UpdateCommentRequest,
  UpdateReplyRequest, CommentFilterRequest, ModifiedCommentStatusRequest
} from '../models/comment.model';

@Injectable({ providedIn: 'root' })
export class CommentService {
  private base = `${environment.apiUrl}/comment`;

  constructor(private http: HttpClient) {}

  addComment(req: AddCommentRequest): Observable<any> {
    return this.http.post(`${this.base}/add`, req);
  }

  replyComment(req: ReplyCommentRequest): Observable<any> {
    return this.http.post(`${this.base}/reply`, req);
  }

  getByPostId(req: CommentFilterRequest): Observable<any> {
    return this.http.get(`${this.base}/get/post`, { params: req as any });
  }

  getReplies(commentId: string, skip: number, count: number): Observable<any> {
    return this.http.get(`${this.base}/get/replies/${commentId}`, { params: { skip, count } });
  }

  deleteComment(id: string): Observable<any> {
    return this.http.delete(`${this.base}/comment/${id}`);
  }

  deleteReply(id: string): Observable<any> {
    return this.http.delete(`${this.base}/reply/${id}`);
  }

  updateComment(req: UpdateCommentRequest): Observable<any> {
    return this.http.put(`${this.base}/comment/update`, req);
  }

  updateReply(req: UpdateReplyRequest): Observable<any> {
    return this.http.put(`${this.base}/reply/update`, req);
  }

  upvoteComment(commentId: string): Observable<any> {
    return this.http.post(`${this.base}/upvote-comment/${commentId}`, {});
  }

  downvoteComment(commentId: string): Observable<any> {
    return this.http.post(`${this.base}/downvote-comment/${commentId}`, {});
  }

  upvoteReply(replyId: string): Observable<any> {
    return this.http.post(`${this.base}/upvote-reply/${replyId}`, {});
  }

  downvoteReply(replyId: string): Observable<any> {
    return this.http.post(`${this.base}/downvote-reply/${replyId}`, {});
  }

  getCommentById(id: string): Observable<any> {
    return this.http.get(`${this.base}/commment/${id}`);
  }

  getReplyById(id: string): Observable<any> {
    return this.http.get(`${this.base}/reply-comment/${id}`);
  }
}
