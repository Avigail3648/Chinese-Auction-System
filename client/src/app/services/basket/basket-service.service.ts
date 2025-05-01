import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketResponse } from '../../models/BasketResponse.model';
import { Basket } from '../../models/Basket.model';
import { CardWhithIds } from '../../models/cardWithIds.model';

@Injectable({
  providedIn: 'root'
})

export class BasketServiceService {
  BASKET_URL = 'http://localhost:5127/api/Baskets'
  CARD_URL='http://localhost:5127/api/Cards'

  constructor() { }

  http: HttpClient = inject(HttpClient);

  getAllBaskets(): Observable<BasketResponse[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({ 'Authorization': `Bearer ${token}` })
    var theBaskets = this.http.get<BasketResponse[]>(this.BASKET_URL, { headers });
    return theBaskets;
  }

  deleteBasket(basketId:number):Observable<Basket>{
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({ 'Authorization': `Bearer ${token}` })
    var theBaskets = this.http.delete<Basket>(this.BASKET_URL+'/' +basketId,{headers});
    return theBaskets;
  }

  addToBasket(giftId: number): Observable<Basket>{
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({ 'Authorization': `Bearer ${token}` })
     return this.http.post<Basket>(this.BASKET_URL+"/"+giftId,{},{ headers })
  }

  addToCards(allCards:CardWhithIds[]): Observable<CardWhithIds[]>{
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({ 'Authorization': `Bearer ${token}` })
     return this.http.post<CardWhithIds[]>(this.CARD_URL, allCards,{ headers })
  }
}
