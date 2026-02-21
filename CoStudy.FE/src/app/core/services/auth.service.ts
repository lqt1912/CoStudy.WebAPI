import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AuthenticateRequest,
  AuthenticateResponse,
  ForgotPasswordRequest,
  ResetPasswordRequest,
  GoogleAuthenticationRequest
} from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly storageKey = 'costudy_auth';
  private authSubject = new BehaviorSubject<AuthenticateResponse | null>(this.loadFromStorage());
  auth$ = this.authSubject.asObservable();

  constructor(private http: HttpClient) {}

  get currentAuth(): AuthenticateResponse | null {
    return this.authSubject.value;
  }

  get isLoggedIn(): boolean {
    return !!this.currentAuth?.jwtToken;
  }

  get isAdmin(): boolean {
    return this.currentAuth?.role === 'Admin';
  }

  get token(): string | null {
    return this.currentAuth?.jwtToken ?? null;
  }

  login(req: AuthenticateRequest): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/accounts/login`, req).pipe(
      tap(res => this.setAuth(res?.result))
    );
  }

  googleLogin(req: GoogleAuthenticationRequest): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/accounts/google-login`, req).pipe(
      tap(res => this.setAuth(res?.result))
    );
  }

  refreshToken(): Observable<any> {
    const token = this.currentAuth?.refreshToken;
    return this.http.post<any>(`${environment.apiUrl}/accounts/refresh-token?refreshToken=${token}`, {}).pipe(
      tap(res => this.setAuth(res?.result))
    );
  }

  forgotPassword(req: ForgotPasswordRequest): Observable<any> {
    return this.http.post(`${environment.apiUrl}/accounts/forgot-password`, req);
  }

  resetPassword(req: ResetPasswordRequest): Observable<any> {
    return this.http.post(`${environment.apiUrl}/accounts/reset-password`, req);
  }

  verifyEmail(token: string): Observable<any> {
    return this.http.post(`${environment.apiUrl}/accounts/verify-email`, null, {
      params: { token }
    });
  }

  logout(): void {
    localStorage.removeItem(this.storageKey);
    this.authSubject.next(null);
  }

  private setAuth(data: AuthenticateResponse): void {
    localStorage.setItem(this.storageKey, JSON.stringify(data));
    this.authSubject.next(data);
  }

  private loadFromStorage(): AuthenticateResponse | null {
    try {
      const raw = localStorage.getItem(this.storageKey);
      return raw ? JSON.parse(raw) : null;
    } catch {
      return null;
    }
  }
}
