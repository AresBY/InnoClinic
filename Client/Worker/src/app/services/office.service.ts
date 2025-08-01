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
  private readonly baseUrl = environment.officeUrl;

  constructor(private http: HttpClient) {}

  createOffice(command: CreateOffice): Observable<string> {
    return this.http.post<string>(`${this.baseUrl}/Create`, command);
  }

  getAllOffices(): Observable<Office[]> {
    return this.http.get<Office[]>(`${this.baseUrl}/GetAll`);
  }

  getOfficeById(id: string): Observable<OfficeDetails> {
    return this.http.get<OfficeDetails>(`${this.baseUrl}/Get/${id}`);
  }

  updateOffice(officeId: string, editedOffice: OfficeDetails): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/UpdateOffice/${officeId}`, editedOffice);
  }
}
