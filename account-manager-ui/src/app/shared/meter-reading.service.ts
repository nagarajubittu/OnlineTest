import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { MeterReading, PostMeterReading } from './meter-reading.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MeterReadingService {

  formData: PostMeterReading = new PostMeterReading();
  filterAccountId = 0;
  readonly baseURL = 'http://localhost:55000';
  list: MeterReading[] = [];

  constructor(private http: HttpClient) { }

  postMeterReading(): Observable<any> {
    return this.http.post(`${this.baseURL}/meter-reading`, this.formData);
  }

  deleteMeterReading(id: number): Observable<any> {
    return this.http.delete(`${this.baseURL}/meter-reading/${id}`);
  }

  postFile(fileToUpload: File): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('file', fileToUpload);
    return this.http
      .post(`${this.baseURL}/meter-reading-uploads`, formData);
}

  refreshList(): void {
    this.http.get(`${this.baseURL}/meter-reading/${this.filterAccountId}/account`)
      .toPromise()
      .then(res => this.list = res as MeterReading[]);
  }
}
