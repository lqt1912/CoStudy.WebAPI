import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError, BehaviorSubject, filter, take, map } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const token = authService.token;

  let cloned = token ? addToken(req, token) : req;

  return next(cloned).pipe(
    // Bắt body-level 401: backend trả HTTP 200 nhưng body có {code: 401}
    map(event => {
      if (event instanceof HttpResponse) {
        const body = event.body as any;
        if (body && (body.code === 401 || (!body.success && body.message === 'Unauthorized'))) {
          throw new HttpErrorResponse({ status: 401, url: req.url });
        }
      }
      return event;
    }),
    catchError(err => {
      if (err instanceof HttpErrorResponse && err.status === 401) {
        return handle401(cloned, next, authService, router);
      }
      return throwError(() => err);
    })
  );
};

function addToken(req: HttpRequest<unknown>, token: string): HttpRequest<unknown> {
  return req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
}

function handle401(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
  authService: AuthService,
  router: Router
) {
  // Nếu không có refresh token → logout và về login ngay
  if (!authService.currentAuth?.refreshToken) {
    authService.logout();
    router.navigate(['/login']);
    return throwError(() => new Error('Unauthorized'));
  }

  if (isRefreshing) {
    return refreshTokenSubject.pipe(
      filter(t => t !== null),
      take(1),
      switchMap(t => next(addToken(req, t!)))
    );
  }

  isRefreshing = true;
  refreshTokenSubject.next(null);

  return authService.refreshToken().pipe(
    switchMap(res => {
      // AuthService.refreshToken() taps res?.result và lưu vào storage
      // jwtToken mới đã được lưu, lấy lại từ currentAuth
      const newToken = authService.token;
      isRefreshing = false;
      refreshTokenSubject.next(newToken);
      return next(addToken(req, newToken!));
    }),
    catchError(err => {
      isRefreshing = false;
      authService.logout();
      router.navigate(['/login']);
      return throwError(() => err);
    })
  );
}
