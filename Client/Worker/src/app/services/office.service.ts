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

  public constructor(private http: HttpClient) {}

  public createOffice(command: CreateOffice): Observable<string> {
    return this.http.post<string>(`${this.baseUrl}/CreateOffice`, command);
  }

  public getAllOffices(): Observable<Office[]> {
    return this.http.get<Office[]>(`${this.baseUrl}/GetOfficeAll`);
  }

  public getOfficeById(id: string): Observable<OfficeDetails> {
    return this.http.get<OfficeDetails>(`${this.baseUrl}/GetOffice/${id}`);
  }

  public updateOffice(officeId: string, editedOffice: OfficeDetails): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/UpdateOffice/${officeId}`, editedOffice);
  }
}
