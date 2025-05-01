import { Component, inject } from '@angular/core';
import { Gift } from '../../models/gift.model';
import { CardServiceService } from '../../services/card/card-service.service';
import { GiftWithCards } from '../../models/giftWhitCards.model';
import { Card } from '../../models/card.model';
import { TableRowCollapseEvent, TableRowExpandEvent } from 'primeng/table';
import { ConfirmationService, MessageService } from 'primeng/api';
import * as XLSX from 'xlsx'

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  providers: [ConfirmationService],

  styleUrl: './card.component.css'
})
export class CardComponent {

  constructor(private CardService: CardServiceService) { }
  gifts!: Gift[]
  allGiftWithCards: GiftWithCards[] = []
  expandedRows: { [key: number]: boolean } = {}
  revenueForGifts: number[] = []
  monee: number = 0
  moneRevenue: number = 0
  allRevenue: string = ''

  ngOnInit() {
    this.getData()
    this.expandedRows = {};
  }

  ngAfterViewInit() {
    this.initializeExpandedRows();
  }

  getData() {
    this.CardService.getAllGifts().subscribe((data) => {
      this.gifts = data
      this.gifts.forEach((gift) => {
        const giftWithCards = new GiftWithCards()
        giftWithCards.gift = gift
        this.CardService.getAllCardsOfGiftByGiftId(gift.giftId).subscribe((cardsOfGift) => {
          giftWithCards.cards = cardsOfGift;
          this.allGiftWithCards.push(giftWithCards);
        })
      })
    })
    setTimeout(() => this.initializeExpandedRows(), 0)
  }
  toggleRowExpansion(gift: GiftWithCards) {
    const giftId = gift.gift.giftId;
    if (this.expandedRows[giftId]) {
      delete this.expandedRows[giftId];
    } else {
      this.expandedRows[giftId] = true;
    }
  }
  
  initializeExpandedRows(): void {
    this.expandedRows = this.allGiftWithCards.reduce((acc, giftWithCards) => {
      acc[giftWithCards.gift.giftId] = true; 
      return acc;
    }, {} as { [key: number]: boolean });
  }

  exportToExcel(): void {
      this.allGiftWithCards.forEach(giftt => this.revenueForGifts[this.monee++] = giftt.cards.length)
      this.monee = 0;
      this.CardService.getAllRevenue().subscribe((revenue) => {
      this.allRevenue = revenue.toString()
      const dataToExport = this.gifts.map(thisGift => {
        return {
          'קוד מתנה': thisGift.giftId.toString(),
          'שם מתנה': thisGift.name,
          'מספר הרכישות למתנה זו': this.revenueForGifts[this.monee++].toString(),
          'סה"כ רווחים ממתנה זו': (this.revenueForGifts[this.moneRevenue++] * thisGift.cardPrice).toString(),
          'סה"כ הכנסות בכל המכירה הסינית': ""
        };
      });

      dataToExport.push({
        'קוד מתנה': "",
        'שם מתנה': "",
        'מספר הרכישות למתנה זו': "",
        'סה"כ רווחים ממתנה זו': "",
       'סה"כ הכנסות בכל המכירה הסינית':this.allRevenue

      })
      const ws = XLSX.utils.book_new();
      const wb = XLSX.utils.json_to_sheet(dataToExport);

      wb['!cols'] = [{ wpx: 120 }, { wpx: 150 }, { wpx: 200 }, { wpx: 150 },{ wpx: 180 }];
      wb['!rows'] = [{ hpx: 20 }, { hpx: 20 }];
      wb['!customHeader'] = { RTL: true };

      XLSX.utils.book_append_sheet(ws, wb, 'הכנסות');
      XLSX.writeFile(ws, 'דוח הכנסות המכירה .xlsx');
    })
  }
}