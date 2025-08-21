import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  // The intercept method runs for every outgoing HTTP request
  public intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        // Log the raw error for debugging
        console.error('HTTP error intercepted:', error);

        let message = 'An unknown error occurred';
        if (error.status === 0) {
          // Network error or CORS issue
          message = 'No connection to the server';
        } else if (error.status >= 400 && error.status < 500) {
          // Client-side error (e.g., 400 Bad Request, 404 Not Found)
          message = `Client error: ${error.error?.message || error.message}`;
        } else if (error.status >= 500) {
          // Server-side error
          message = 'Server error. Please try again later';
        }

        // For now, display as alert (can be replaced with Snackbar/Toast service)
        alert(message);

        // Rethrow the error so the caller can still handle it if needed
        return throwError(() => error);
      })
    );
  }
}
