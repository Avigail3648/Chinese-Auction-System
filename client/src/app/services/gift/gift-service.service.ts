import { Injectable, inject } from '@angular/core';
import { Gift } from '../../models/gift.model';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { observableToBeFn } from 'rxjs/internal/testing/TestScheduler';
import { Card } from '../../models/card.model';

@Injectable({
  providedIn: 'root'
})
export class GiftServiceService {
  GIFT_URL = 'http://localhost:5127/api/Gifts';
  LOTTERY_URL = 'http://localhost:5127/api/Lottery';
  CARD_URL = "http://localhost:5127/api/Cards";

  constructor() { }

  http: HttpClient = inject(HttpClient);

  getAll(): Observable<Gift[]> {
    return this.http.get<Gift[]>(this.GIFT_URL);
  }

  getById(id: number): Observable<Gift> {
    return this.http.get<Gift>(this.GIFT_URL + '/' + id);
  }

  update(g: Gift): Observable<Gift[]> {
    const token = localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`)
    return this.http.put<Gift[]>(this.GIFT_URL, g, { headers });

  }

  add(g: Gift): Observable<Gift[]> {
    const token = localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`)
    return this.http.post<Gift[]>(this.GIFT_URL, g, { headers });

  }
  randomGift(g: Gift): Observable<Card> {
    const token = localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`)
    return this.http.get<Card>(this.LOTTERY_URL + '/' + g.giftId, { headers })
  }

  delete(id: number): Observable<Gift[]> {
    const token = localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`)
    return this.http.delete<Gift[]>(this.GIFT_URL + '/' + id, { headers });
  }

  getCategoriesNames(): Observable<string[]> {
    return this.http.get<string[]>(this.GIFT_URL + '/CategoriesNames')
  }
  getAllCardsOfGiftByGiftId(giftId: number): Observable<Card[]> {
    const token = localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`)
    var allCardOfGift = this.http.get<Card[]>(this.CARD_URL + '/' + giftId, { headers });
    return allCardOfGift;
  }
  getAllWinCard(): Observable<Card[]> {
    const token = localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`)
    return this.http.get<Card[]>(this.CARD_URL + "/" + "getAllWinCard", { headers })
  }
}
