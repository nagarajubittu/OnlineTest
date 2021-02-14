import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Account } from './account.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  formData: Account = new Account();
  editAccount = false;
  readonly baseURL = 'http://localhost:5000';
  list: Account[] = [];

  constructor(private http: HttpClient) { }

  postAccount(): Observable<any> {
    return this.http.post(`${this.baseURL}/account`, this.formData);
  }
  putAccount(): Observable<any> {
    return this.http.put(`${this.baseURL}/account`, this.formData);
  }
  deleteAccount(id: number): Observable<any> {
    return this.http.delete(`${this.baseURL}/account/${id}`);
  }

  refreshList(): void {
    this.http.get(`${this.baseURL}/account`)
      .toPromise()
      .then(res => this.list = res as Account[]);
  }
}
