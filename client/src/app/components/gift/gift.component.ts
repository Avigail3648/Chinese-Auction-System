import { Component, OnInit, Renderer2, inject } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Gift } from '../../models/gift.model';
import { GiftServiceService } from '../../services/gift/gift-service.service';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Donor } from '../../models/donor.model';
import { DonorServiceService } from '../../services/donor/donor-service.service';
import { Card } from '../../models/card.model';
import { CardServiceService } from '../../services/card/card-service.service';
import { GiftNumPurchases } from '../../models/giftNumPurchases.model';
import * as XLSX from 'xlsx';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-gift',
  templateUrl: './gift.component.html',
  styleUrl: './gift.component.css',
  providers: [MessageService, ConfirmationService],
  styles: [
    `:host ::ng-deep .p-dialog .product-image {
          width: 150px;
          margin: 0 auto 2rem auto;
          display: block;
      }`
  ]
})
export class GiftComponent {

  GiftService: GiftServiceService = inject(GiftServiceService);
  DonorService: DonorServiceService = inject(DonorServiceService);
  CardService: CardServiceService = inject(CardServiceService);
  router: Router = inject(Router)
  touchName: boolean = false;
  touchImage: boolean = false;
  giftDialog: boolean = false;
  gifts$!: Gift[]
  gift!: Gift;
  selectedGifts!: Gift[] | null;
  submitted: boolean = false;
  donors$!: string[]
  giftsNames: string[] = []
  categoriesName: string[] = []
  winnerCards: Card[] = []
  winnerGifts: string[] = []
  nameBefore: string = ""
  edit: boolean = false
  filterName: string = ""
  arrFilterBy: string[] = ["שם", "תורם", "מספר רכישות"];
  allGiftsWithNumPurchases: GiftNumPurchases[] = []
  GiftsNumPurchasesToShow: GiftNumPurchases[] = []
  winCards: Card[] = []
  tmpgiftName: string = '';

  constructor(private messageService: MessageService, private confirmationService: ConfirmationService) { }

  ngOnInit() {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd && event.urlAfterRedirects === '/gift') {
        window.location.reload();
      }
    });
    this.GiftService.getAll().subscribe((g) => {
      this.gifts$ = g
      this.giftsNames = this.gifts$.map(e => e.name)
    })
    this.DonorService.getAll().subscribe((donors) => {
      this.donors$ = donors.map((d) => d.name);
    })
    this.GiftService.getCategoriesNames().subscribe((categories) => {
      this.categoriesName = categories
    })
    this.winnerGifts = []
    this.CardService.getAllWinCards().subscribe((cards) => {
      this.winnerCards = cards
      this.winnerCards.map((c) => this.winnerGifts.push(c.gift.name))
    })
    this.GiftService.getAll().subscribe((allGift) => {
      this.gifts$ = allGift;
      this.giftsNames = allGift.map(g => g.name)
      this.gifts$.map(g => this.GiftService.getAllCardsOfGiftByGiftId(g.giftId).subscribe((cardsOfGift) => {
        var curr = new GiftNumPurchases()
        curr.NumPurchases = cardsOfGift.length
        curr.giftId = g.giftId
        this.allGiftsWithNumPurchases.push(curr)
      }))
    })
    this.GiftService.getCategoriesNames().subscribe((categories) => {
      this.categoriesName = categories;
    })
    this.getAllWinCard()
  }
  
  getAllWinCard() {
    this.GiftService.getAllWinCard().subscribe((wins) => {
      this.winCards = wins
    })
  }

  openNew() {
    this.gift = new Gift();
    this.submitted = false;
    this.giftDialog = true;
  }

  editGift(gift: Gift) {
    this.edit = true;
    this.nameBefore = gift.name
    this.gift = { ...gift };
    this.giftDialog = true;
  }

  hideDialog() {
    this.touchName = false;
    this.touchImage = false;
    this.giftDialog = false;
    this.submitted = false;
  }

  deleteSelectedGifts() {
    this.confirmationService.confirm({
      message: 'האם אתה בטוח שברצונך למחוק את הפריטים שנבחרו?',
      header: 'הודעה',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.selectedGifts?.forEach(e => {
          this.GiftService.delete(e.giftId).subscribe((giftAfterDeleted) => {
            this.gifts$ = giftAfterDeleted
            this.giftsNames = this.gifts$.map(e => e.name)
          })
        });
        this.selectedGifts = null;
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'המתנות שנבחרו נמחקו בהצלחה', life: 3000 });
      }
    });
  }

  deleteGift(gift: Gift) {
    this.confirmationService.confirm({
      message: ' האם אתה בטוח שברצונך למחוק' + " " + gift.name + '?',
      header: 'הודעה',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.GiftService.delete(gift.giftId).subscribe((giftAfterDeleted) => {
          this.gifts$ = giftAfterDeleted
          this.giftsNames = this.gifts$.map(e => e.name)
          this.gift = new Gift()
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'מתנה נמחקה בהצלחה', life: 3000 });

        }, (error) => {
          let errorMessage = 'שגיאה';
          if (error.error) {
            errorMessage = error.error
          }
          this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: errorMessage, life: 3000 });
        }
        )
      }
    });
  }

  saveGift() {
    this.hideDialog()
    if (this.gift.name?.trim()) {
      if (this.gift.giftId) {
        this.GiftService.update(this.gift).subscribe((giftAfterDeleted) => {
          this.gifts$ = giftAfterDeleted
          this.giftsNames = this.gifts$.map(e => e.name)
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'מתנה עודכנה בהצלחה', life: 3000 });
        }, (error) => {
          let errorMessage = 'שגיאה';
          if (error.error) {
            errorMessage = error.error
          }
          this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: errorMessage, life: 3000 });
        })
      } else {
        this.gift.giftId = 0
        this.GiftService.add(this.gift).subscribe((giftAfterDeleted) => {
          this.gifts$ = giftAfterDeleted
          this.giftsNames = this.gifts$.map(e => e.name)
        })
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'מתנה נוספה בהצלחה', life: 3000 });
      }
      this.giftDialog = false;
      this.gift = new Gift();
    }
  }


  onGiftNameChange(searchText: any) {
    this.tmpgiftName = searchText;
    if (!this.tmpgiftName.trim()) {
      this.refreshSerch();
    }
    else {
      this.GiftService.getAll().subscribe((g) => {
        this.gifts$ = g;
        switch (this.filterName) {
          case "שם": {
            this.gifts$ = this.gifts$.filter(gift => gift.name.includes(this.tmpgiftName));
            break;
          }
          case "תורם": {
            this.gifts$ = this.gifts$.filter(gift => gift.donorName.includes(this.tmpgiftName))
            break;
          }
          case "מספר רכישות": {
            this.gifts$ = []
            this.GiftsNumPurchasesToShow = this.allGiftsWithNumPurchases.filter(gift => gift.NumPurchases == searchText);
            this.GiftsNumPurchasesToShow.map(g => this.GiftService.getById(g.giftId).subscribe((gift) => {
              this.gifts$.push(gift)
            }))
            break;
          }
        }
      });
    }
  }

  refreshSerch() {
    this.GiftService.getAll().subscribe((g) => {
      this.gifts$ = g;
    })
    this.gift = new Gift();
    this.tmpgiftName = '';
  }

  random(gift: Gift) {
    this.GiftService.randomGift(gift).subscribe((card) => {
      this.CardService.getAllWinCards().subscribe((cards) => {
        this.winnerCards = cards
        this.winnerCards.map((c) => this.winnerGifts.push(c.gift.name))
      })
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: card.user.name + " זכתה ב " + card.gift.name, life: 3000 });

    },
      (error) => {
        let errorMessage = 'הגרלה נכשלה';
        errorMessage = error['error']
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: errorMessage,
        });
      })
  }

  exportToExcel(): void {
      const dataToExport = this.winCards.map(thisCard => {
        return {
          'שם מתנה': thisCard.gift.name,
          'זוכה': thisCard.user.name,
        };
      });

      const ws = XLSX.utils.book_new();
      const wb = XLSX.utils.json_to_sheet(dataToExport);

      wb['!cols'] = [{ wpx: 120 }, { wpx: 150 }, { wpx: 200 }, { wpx: 150 },{ wpx: 180 }];
      wb['!rows'] = [{ hpx: 20 }, { hpx: 20 }];
      wb['!customHeader'] = { RTL: true };

      XLSX.utils.book_append_sheet(ws, wb, 'זוכים במתנות');
      XLSX.writeFile(ws, 'דוח זוכים במתנות .xlsx');
  }
}