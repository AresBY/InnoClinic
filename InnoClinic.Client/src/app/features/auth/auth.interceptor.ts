import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, switchMap, filter, take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthService } from '@services/auth.service';
import { environment } from '@environments/environment';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject = new BehaviorSubject<string | null>(null);

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const isAuthUrl =
      req.url.includes(`${environment.authUrl}/SignIn`) ||
      req.url.includes(`${environment.authUrl}/RefreshToken`);

    if (isAuthUrl) {
      return next.handle(req);
    }

    const accessToken = this.authService.getAccessToken();

    const authReq = accessToken
      ? req.clone({
          setHeaders: { Authorization: `Bearer ${accessToken}` },
          withCredentials: true
        })
      : req.clone({ withCredentials: true });

    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          return this.handle401Error$(authReq, next);
        }
        return throwError(() => error);
      })
    );
  }

  private handle401Error$(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.authService.refreshToken$().pipe(
        switchMap((newToken: string) => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(newToken);

          const retryReq = request.clone({
            setHeaders: {
              Authorization: `Bearer ${newToken}`
            },
            withCredentials: true
          });
          return next.handle(retryReq);
        }),
        catchError((err) => {
          this.isRefreshing = false;
          this.authService.logout();
          this.router.navigate(['/sign-in']);
          return throwError(() => err);
        })
      );
    } else {
      return this.refreshTokenSubject.pipe(
        filter((token) => token !== null),
        take(1),
        switchMap((token) => {
          const retryReq = request.clone({
            setHeaders: {
              Authorization: `Bearer ${token!}`
            },
            withCredentials: true
          });
          return next.handle(retryReq);
        })
      );
    }
  }
}
