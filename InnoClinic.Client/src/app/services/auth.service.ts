import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterRequest } from '../models/register-request.model';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) {}

  register(data: RegisterRequest) {
    return this.http.post(`${environment.authUrl}/Register`, data);
  }

  checkEmailExists(email: string): Observable<any> {
    const params = new HttpParams().set('email', email.toLowerCase());
    return this.http.get(`${environment.authUrl}/CheckEmailExists`, { params });
  }

  confirmEmail$(userId: string): Observable<any> {
    return this.http.post(`${environment.authUrl}/ConfirmEmail`, { userId });
  }
}
