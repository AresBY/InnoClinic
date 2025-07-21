import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterRequest } from '../models/register-request.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = '/api/Auth/Register';

  constructor(private http: HttpClient) {}

  register(data: RegisterRequest) {
    return this.http.post(this.apiUrl, data);
  }

  checkEmailExists(email: string): Observable<any> {
    const params = new HttpParams().set('email', email.toLowerCase());
    return this.http.get('/api/Auth/CheckEmailExists', { params });
  }
}
