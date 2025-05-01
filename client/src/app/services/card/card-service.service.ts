import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Card } from '../../models/card.model';
import { Gift } from '../../models/gift.model';

@Injectable({
  providedIn: 'root'
})
export class CardServiceService {

  constructor() { }
  CARD_URL= 'http://localhost:5127/api/Cards';
  GIFT_URL = "http://localhost:5127/api/Gifts";

  http: HttpClient = inject(HttpClient);

  getAllCardsOfGiftByGiftId(giftId:number): Observable<Card[]> {
    const token=localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}` )
    var allCardOfGift= this.http.get<Card[]>(this.CARD_URL + "/"+giftId,{headers});
    return allCardOfGift;
  }

  getAllWinCards(): Observable<Card[]> {
    const token=localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}` )
    return this.http.get<Card[]>(this.CARD_URL+'/getAllWinCard',{headers});
  }

  getAllGifts(): Observable<Gift[]> {
    const token=localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}` )
    return this.http.get<Gift[]>(this.GIFT_URL,{headers});
  }

  getAllCardsByGiftID(giftId: number):Observable<Card[]>{
    const token=localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}` )
    return this.http.get<Card[]>(this.CARD_URL+'/'+giftId,{headers});
  }
  getAllRevenue():Observable<number>{
    const token=localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}` )
    var revenue = this.http.get<number>(this.CARD_URL+"/"+"getAllRevenue",{headers})
    return revenue
  }
}
