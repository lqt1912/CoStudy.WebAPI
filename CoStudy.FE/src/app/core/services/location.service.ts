import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class LocationService {
  private base = `${environment.apiUrl}/location`;

  constructor(private http: HttpClient) {}

  getProvinces(name?: string): Observable<any> {
    return this.http.get(`${this.base}/province`, { params: name ? { name } : {} });
  }

  getProvinceByCode(code: string): Observable<any> {
    return this.http.get(`${this.base}/province/${code}`);
  }

  getDistrictsByProvince(province: string): Observable<any> {
    return this.http.get(`${this.base}/district/province/${province}`);
  }

  getDistrictByCode(code: string): Observable<any> {
    return this.http.get(`${this.base}/district/${code}`);
  }

  getWardsByDistrict(district: string): Observable<any> {
    return this.http.get(`${this.base}/ward/district/${district}`);
  }

  getWardByCode(code: string): Observable<any> {
    return this.http.get(`${this.base}/ward/${code}`);
  }
}
