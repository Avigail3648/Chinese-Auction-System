import { Component } from '@angular/core';
import { Donor } from '../../models/donor.model';
import { DonorServiceService } from '../../services/donor/donor-service.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { catchError, tap, throwError } from 'rxjs';
import { Gift } from '../../models/gift.model';
import { FormControl, FormGroup } from '@angular/forms';


@Component({
  selector: 'app-donor',
  templateUrl: './donor.component.html',
  styleUrls: ['./donor.component.css'],
  providers: [MessageService, ConfirmationService],
})

export class DonorComponent {
  donors!: Donor[];
  cloneddonors: { [s: number]: Donor } = {};
  gifts: Gift[] = []
  showGift: boolean = false;
  donorDialog: boolean = false;
  submitted: boolean = false;
  Adddonor: Donor = new Donor();
  touchEmail: boolean = false;
  touchName: boolean = false;
  tmpgiftName: string = '';

  constructor(
    private donorService: DonorServiceService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) { }

  ngOnInit() {
    this.donorService.getAll().subscribe(
      (data) => {
        this.donors = data;
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'שגיאה',
          detail: 'שגיאה בהצגת תורמים',
        });
      }
    );
  }
  openNew() {
    this.Adddonor = new Donor();
    this.submitted = false;
    this.donorDialog = true;
  }
  hideDialog() {
    this.donorDialog = false;
    this.submitted = false;
    this.touchEmail = false;
    this.touchName = false;
  }
  saveDonor() {
    this.hideDialog()
    this.donorService.add(this.Adddonor).pipe(
      tap((donorsAfterCreate) => {
        this.donors = donorsAfterCreate;
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: `התורם "${this.Adddonor.name}" נוסף בהצלחה.`,
        });
      }),
      catchError((error) => {
        const donorId = this.Adddonor.donorId as number;
        this.donors = this.donors.map((d) =>
          d.donorId === donorId ? this.cloneddonors[donorId] : d
        );
        delete this.cloneddonors[donorId];
        let errorMessage = 'שגיאה';
        if (error.error && error.error.errors) {
          const errors = error.error.errors;
          errorMessage = Object.keys(errors)
            .map((key) => `${key}: ${errors[key].join(', ')}`)
            .join('\n');
        }

        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: errorMessage,
        });

        return throwError(error);
      })
    )
      .subscribe();
  }

  openGifts(donor: Donor) {
    this.donorService.getGiftByDonorID(donor.donorId).subscribe((gifts) => {
      this.gifts = gifts;
    })
    this.showGift = true;
  }

  onRowEditInit(donor: Donor) {
    this.cloneddonors[donor.donorId as number] = { ...donor };
  }

  onRowEditSave(donor: Donor) {
    this.donorService.update(donor).subscribe(
      (updatedDonors) => {
        this.donors = updatedDonors;
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'פרטי תורם עודכנו בהצלחה',
        });
      },
      (error) => {
        const donorId = donor.donorId as number;

        this.donors = this.donors.map((d) =>
          d.donorId === donorId ? this.cloneddonors[donorId] : d
        );
        delete this.cloneddonors[donorId];
        let errorMessage = 'שגיאה';
        if (error.error && error.error.errors) {
          const errors = error.error.errors;
          if (errors.Phone) {
            errorMessage = 'הפלאפון חייב להיות באורך 10 תווים בדיוק';
          }
          if (errors.Mail) {
            errorMessage = 'מייל לא תקין';
          }
        }
        else if (error.error && error.error.errors) {
          const errors = error.error.errors;
          errorMessage = Object.keys(errors)
            .map((key) => `${key}: ${errors[key].join(', ')}`)
            .join('\n');
        }
        this.messageService.add({
          severity: 'error',
          summary: 'שגיאה',
          detail: errorMessage,
        });
      }
    );
  }

  onRowEditCancel(donor: Donor, index: number) {
    this.donors[index] = this.cloneddonors[donor.donorId as number];
    delete this.cloneddonors[donor.donorId as number];
  }

  deleteDonor(donor: Donor) {
    this.confirmationService.confirm({
      message: `האם אתה בטוח שברצונך למחוק את "${donor.name}"?`,
      header: 'הודעה',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.donorService.delete(donor.donorId as number)
          .pipe(
            tap((updatedDonors) => {
              this.donors = updatedDonors;
              this.messageService.add({
                severity: 'success',
                summary: 'Success',
                detail: `התורם "${donor.name}" נמחק בהצלחה`,
              });
            }),
            catchError((error) => {
              let errorMessage = 'שגיאה';
              if (error.error) {
                errorMessage = error.error
                this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: errorMessage, life: 1000 });
              }
              return throwError(error);
            })
          )
          .subscribe();
      }
    });

  }


  onDonorNameChange(searchText: string) {
    this.tmpgiftName = searchText;
    if (!this.tmpgiftName.trim()) {
      this.refreshSerch();
    }
    else {
      this.donorService.getAll().subscribe((d) => {
        this.donors = d;
        this.donors = this.donors.filter(donor =>
          donor.name.toLowerCase().includes(this.tmpgiftName.toLowerCase())
        );

      })
    }
  }

  refreshSerch() {
    this.donorService.getAll().subscribe((d) => {
      this.donors = d;
    })
  }
}
