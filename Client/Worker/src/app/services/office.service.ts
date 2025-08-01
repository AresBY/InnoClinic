import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { CreateOffice } from '@models/create-office.model';
import { OfficeDetails } from '@models/office-details.model';
import { Office } from '@models/office.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OfficeService {
  constructor(private http: HttpClient) {}

  createOffice(command: CreateOffice): Observable<string> {
    return this.http.post<string>(`${environment.officeUrl}/Create`, command);
  }

  getAllOffices(): Observable<Office[]> {
    return this.http.get<Office[]>(`${environment.officeUrl}/GetAll`);
  }

  getOfficeById(id: string): Observable<OfficeDetails> {
    return this.http.get<OfficeDetails>(`${environment.officeUrl}/Get/${id}`);
  }
}
