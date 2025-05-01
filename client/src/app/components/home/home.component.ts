import { Component } from '@angular/core';
import { Gift } from '../../models/gift.model';
import { Donor } from '../../models/donor.model';
import { BasketResponse } from '../../models/BasketResponse.model';
import { GiftServiceService } from '../../services/gift/gift-service.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BasketServiceService } from '../../services/basket/basket-service.service';
import { jwtDecode } from "jwt-decode";
import { CardServiceService } from '../../services/card/card-service.service';
import { Card } from '../../models/card.model';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  providers: [ConfirmationService],

})
export class HomeComponent {
  giftDialog!: boolean;
  giftDialog1!: boolean;
  flagUpdate: boolean = false;
  gifts: Gift[] = [];
  gift!: Gift;
  giftt!: Gift;
  SelectedGifts: Gift[] = [];
  donors: Donor[] = [];
  submitted!: boolean;
  maskBasket = new BasketResponse();
  winners: Card[] = []
  card: Card | undefined
  cardWin: Card | undefined
  user: User | undefined
  numbersId: number[] = []
  win: boolean = false;
  constructor(private giftService: GiftServiceService, private messageService: MessageService, private basketService: BasketServiceService,
    private confirmationService: ConfirmationService,
    private cardService: CardServiceService) {
  }
  ngOnInit() {
    this.giftService.getAll().subscribe((data) => {
      this.gifts = data
      this.getAllCardsByGiftID(data)
    });
  }

  addToBasket(giftId: number) {
    const token = localStorage.getItem("token");
    if (token) {
      this.basketService.addToBasket(giftId).subscribe((data) => {
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: "!מתנה נוספה לסל בהצלחה", life: 3000 });
      })
    }
  }

  delete(id: number) {
    this.giftService.delete(id).subscribe((h) => {
      this.giftService.getAll().subscribe((gift1) => {
        this.gifts = gift1
      })
    })
  }

  getAllCardsByGiftID(gifts: Gift[]) {
    gifts.forEach(g => {
      this.cardService.getAllCardsByGiftID(g.giftId).subscribe((data) => {
        this.card = data.find(c => c.isWin == true)
        if (this.card)
          this.winners.push(this.card);
        this.numbersId = this.winners.map(c => c.gift.giftId)
      })
    });
  }

  show(giftId: number) {
    this.cardWin = this.winners.find(w => w.gift.giftId == giftId)
    this.user = this.cardWin?.user
    this.win = true
    this.messageService.add({ severity: 'info', summary: ':שם זוכה', detail: this.user?.name, life: 2000 });
  }
}