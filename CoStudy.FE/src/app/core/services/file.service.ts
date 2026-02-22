import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class FileService {
  private readonly http = inject(HttpClient);
  private readonly uploadUrl = `${environment.apiUrl}/File/file-upload`;

  /**
   * Upload one or more files to Google Cloud Storage via backend.
   * @param files  Native File objects to upload
   * @param folder Destination folder key (default: 'posts')
   * @returns Observable of URL string array from res?.result
   */
  uploadFiles(files: File[], folder = 'posts'): Observable<string[]> {
    const form = new FormData();
    form.append('Folder', folder);
    files.forEach(f => form.append('Files', f, f.name));
    return this.http.post<any>(this.uploadUrl, form).pipe(
      map(res => (res?.result as string[]) ?? [])
    );
  }
}
