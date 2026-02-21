import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError, BehaviorSubject, filter, take } from 'rxjs';
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
      const newToken = res?.data?.jwtToken;
      isRefreshing = false;
      refreshTokenSubject.next(newToken);
      return next(addToken(req, newToken));
    }),
    catchError(err => {
      isRefreshing = false;
      authService.logout();
      router.navigate(['/login']);
      return throwError(() => err);
    })
  );
}
