import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreatePostReportRequest, CreateCommentReportRequest, CreateReplyReportRequest } from '../models/shared.model';
import { BaseGetAllRequest } from '../models/post.model';

@Injectable({ providedIn: 'root' })
export class ReportService {
  private base = `${environment.apiUrl}/report`;

  constructor(private http: HttpClient) {}

  reportPost(req: CreatePostReportRequest): Observable<any> {
    return this.http.post(`${this.base}/report-post`, req);
  }

  reportComment(req: CreateCommentReportRequest): Observable<any> {
    return this.http.post(`${this.base}/report-comment`, req);
  }

  reportReply(req: CreateReplyReportRequest): Observable<any> {
    return this.http.post(`${this.base}/report-reply`, req);
  }

  getAll(req: BaseGetAllRequest): Observable<any> {
    return this.http.post(`${this.base}/all`, req);
  }

  getById(id: string): Observable<any> {
    return this.http.get(`${this.base}/${id}`);
  }

  approveReports(ids: string[]): Observable<any> {
    return this.http.post(`${this.base}/approve-report`, ids);
  }
}
