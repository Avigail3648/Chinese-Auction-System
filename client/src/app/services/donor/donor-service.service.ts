import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Donor } from '../../models/donor.model';
import { Gift } from '../../models/gift.model';


@Injectable({
  providedIn: 'root'
})
export class DonorServiceService {

  DONOR_URL = 'http://localhost:5127/api/Donors';
  GIFT_URL = 'http://localhost:5127/api/Gifts';

  constructor() { }

  http: HttpClient = inject(HttpClient);
  getAll(): Observable<Donor[]> {
    const token = localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`)
    return this.http.get<Donor[]>(this.DONOR_URL, { headers });
  }

  getById(id: number): Observable<Donor> {
    const token = localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`)
    return this.http.get<Donor>(this.DONOR_URL + '/' + id, { headers });
  }

  update(d: Donor): Observable<Donor[]> {
    const token = localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`)
    return this.http.put<Donor[]>(this.DONOR_URL, d, { headers });
  }

  add(d: Donor): Observable<Donor[]> {
    const token = localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`)
    return this.http.post<Donor[]>(this.DONOR_URL, d, { headers });
  }

  getGiftByDonorID(id: Number): Observable<Gift[]> {
    const token = localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`)
    return this.http.get<Gift[]>(this.GIFT_URL + '/getGiftsByDonorID/' + id, { headers });
  }

  delete(id: number): Observable<Donor[]> {
    const token = localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`)
    return this.http.delete<Donor[]>(this.DONOR_URL + '/' + id, { headers });
  }
}
