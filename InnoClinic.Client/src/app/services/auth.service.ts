import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterRequest } from '../models/register-request.model';
import { BehaviorSubject, Observable, tap, map } from 'rxjs';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly accessTokenKey: string = 'access_token';

  private isAuthenticatedSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    this.hasToken()
  );
  public readonly isAuthenticated$: Observable<boolean> =
    this.isAuthenticatedSubject.asObservable();

  constructor(private readonly http: HttpClient) {}

  public register$(data: RegisterRequest): Observable<any> {
    return this.http.post<any>(`${environment.authUrl}/Register`, data);
  }

  public checkEmailExists$(email: string): Observable<any> {
    const params: HttpParams = new HttpParams().set('email', email.toLowerCase());
    return this.http.get<any>(`${environment.authUrl}/CheckEmailExists`, { params });
  }

  public confirmEmail$(userId: string): Observable<any> {
    return this.http.post<any>(`${environment.authUrl}/ConfirmEmail`, { userId });
  }

  public signIn$(credentials: { email: string; password: string }): Observable<any> {
    return this.http
      .post<any>(`${environment.authUrl}/SignIn`, credentials, { withCredentials: true })
      .pipe(tap((response) => this.setAccessToken(response.accessToken)));
  }

  public setAccessToken(token: string): void {
    localStorage.setItem(this.accessTokenKey, token);
    this.isAuthenticatedSubject.next(true);
  }

  public getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  public isLoggedIn(): boolean {
    return this.getAccessToken() !== null;
  }

  public logout(): void {
    localStorage.removeItem(this.accessTokenKey);
    this.isAuthenticatedSubject.next(false);

    this.http.post<any>(`${environment.authUrl}/Logout`, {}, { withCredentials: true }).subscribe({
      next: () => console.log('Logged out successfully'),
      error: (err) => console.error('Logout failed', err)
    });
  }

  public refreshToken$(): Observable<string> {
    return this.http
      .post<{
        accessToken: string;
      }>(`${environment.authUrl}/RefreshToken`, {}, { withCredentials: true })
      .pipe(
        tap((response) => this.setAccessToken(response.accessToken)),
        map((response) => response.accessToken)
      );
  }

  private hasToken(): boolean {
    return !!this.getAccessToken();
  }
}
