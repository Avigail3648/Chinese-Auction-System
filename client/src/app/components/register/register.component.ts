import { Component, inject } from '@angular/core';
import { AuthServiceService } from '../../services/auth/auth-service.service';
import { LoginRequestt } from '../../models/loginRequestt.model';
import { User } from '../../models/user.model';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  providers: [MessageService, ConfirmationService],
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  constructor(private messageService: MessageService,
    private confirmationService: ConfirmationService) {

  }
  password: string = "";
  mail: string = "";
  phone: string = "";
  name: string = "";
  address!: string
  authSrv: AuthServiceService = inject(AuthServiceService)
  user: User = new User();
  res: string = "";
  token: string = ""
  touchPassword: boolean = false;
  touchEmail: boolean = false;
  touchName: boolean = false;
  isAdmin: Boolean = false
  router: Router = inject(Router)
  
  register() {
    this.user.mail = this.mail;
    this.user.password = this.password;
    this.user.UserId = 0; 
    this.user.address = this.address;
    this.user.name = this.name;
    this.user.phone = this.phone;
  
    this.authSrv.register(this.user).subscribe({
      next: (data) => {
        this.token = data.token;
        localStorage.setItem('token', this.token);
          this.authSrv.getRole().subscribe({
          next: (isAdmin) => {
            this.isAdmin = isAdmin;
              if (this.isAdmin) {
              this.navigate('adminDashboard');
            } else {
              this.navigate('giftSale'); 
            }
          },
          error: (err) => {
            console.error('Error fetching role:', err);
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'שגיאה בבדיקת תפקיד, נסה שנית',
            });
          },
        });
      },
      error: (error) => {
        const errorMessage = error.error || 'שגיאה כללית, נסה שנית';
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: errorMessage,
        });
      },
    });
  }
  
  navigate(s: string) {
    this.router.navigate([`/${s}`]);
  }
  
  changeRole() {
    this.authSrv.getRole().subscribe((role) => {
      if (role == true)
        this.authSrv.changeRolee("Admin");
      if (role == false)
        this.authSrv.changeRolee("User");
    })
  }
  loginAsAdmin() {
    this.isAdmin = true;
    this.register();
  }
  
}
