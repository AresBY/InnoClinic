import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

export interface CreateOfficeCommand {
  photoUrl?: string;
  city: string;
  street: string;
  houseNumber: string;
  officeNumber?: string;
  registryPhoneNumber: string;
  status: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class OfficeService {
  constructor(private http: HttpClient) {}

  createOffice(command: CreateOfficeCommand): Observable<string> {
    return this.http.post<string>(`${environment.officeUrl}/CreateOffice`, command);
  }
}
