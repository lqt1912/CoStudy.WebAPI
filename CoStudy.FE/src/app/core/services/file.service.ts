import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class FileService {
  private base = `${environment.apiUrl}/file`;

  constructor(private http: HttpClient) {}

  /**
   * Upload files to Firebase via backend
   * @param folder destination folder name (e.g. 'posts', 'avatars')
   * @param files FileList or File[]
   */
  uploadFiles(folder: string, files: File[]): Observable<any> {
    const form = new FormData();
    form.append('Folder', folder);
    files.forEach(f => form.append('Files', f, f.name));
    return this.http.post(`${this.base}/file-upload`, form);
  }
}
