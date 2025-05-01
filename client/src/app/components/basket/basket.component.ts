import { Component } from '@angular/core';
import { MessageService } from 'primeng/api';
import { BasketResponse } from '../../models/BasketResponse.model';
import { BasketServiceService } from '../../services/basket/basket-service.service';
import { CardWhithIds } from '../../models/cardWithIds.model';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.css',
  providers: [MessageService],

})
export class BasketComponent {
  layout: any = "grid";
  baskets: BasketResponse[] = [];
  empty: boolean=false;
  visible: boolean = false;
  cards:CardWhithIds[]=[]


  constructor(private basketService: BasketServiceService, private messageService: MessageService) { }

  ngOnInit() {
    this.getAllBaskets()
  }

  deleteBasket(basketId: number) {
    this.basketService.deleteBasket(basketId).subscribe((basket) => {    
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: "המתנה נמחקה מהסל בהצלחה ", life: 3000 });
      this.getAllBaskets()
    },
      (error) => {
        let errorMessage = 'מחיקה נכשלה';
        if (error['error'])
          errorMessage = error['error']
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: errorMessage,
        });
      })
  }

  getAllBaskets(){
    this.basketService.getAllBaskets().subscribe((theBaskets) => { 
      this.baskets = theBaskets
      if(this.baskets.length==0)
        this.empty=true;
     });
  }

  showDialog() {
    this.visible = true;
  }

  buy() {
    this.basketService.getAllBaskets().subscribe((theBaskets) => { 
      this.baskets = theBaskets      
      this.baskets.map(b=>{
        const card=new CardWhithIds()
        card.cardId=0
        card.giftId=b.gift.giftId
        card.userId=b.userId
        card.isWin=false
        this.cards.push(card)
      })
      if(this.baskets.length==0)
        this.empty=true;
      this.basketService.addToCards(this.cards).subscribe((theBaskets) => { 
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: "תשלום בוצע בהצלחה", life: 3000 })
        this.getAllBaskets()
      },
                (error)=>{
          this.messageService.add({ severity: 'danger', summary: 'error', detail: "תשלום נכשל", life: 3000 })}, 
      )

     });
    this.visible = false;
    this.getAllBaskets()
  }
}
