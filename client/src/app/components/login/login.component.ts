
import { Component, inject } from '@angular/core';
import { AuthServiceService } from '../../services/auth/auth-service.service';
import { LoginRequestt } from '../../models/loginRequestt.model';
import { MessageService, ConfirmationService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  providers: [MessageService, ConfirmationService],
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  password: string = "";
  mail: string = "";
  authSrv: AuthServiceService = inject(AuthServiceService);
  user: LoginRequestt = new LoginRequestt();
  res: string = "";
  token: string = "";
  touchPassword: boolean = false;
  touchEmail: boolean = false;
  router: Router = inject(Router);
  isAdmin: Boolean = false;

  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) { }

  login() {
    this.user.password = this.password;
    if (this.isAdmin)
      this.user.mail = "*@gmail.com"; 
    else
      this.user.mail = this.mail;  

    this.authSrv.login(this.user).subscribe(
      (data) => {
        this.token = data.token;
        localStorage.setItem('token', this.token);
        this.changeRole(); 
        this.navigate('giftSale');
      },
      (error) => {
        let errorMessage = 'משתמש לא רשום';
        errorMessage = error['error'];
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: errorMessage,
        });
      }
    );
  }

  navigate(s: string) {
    this.router.navigate([`/${s}`]);
  }

  changeRole() {
    this.authSrv.getRole().subscribe((role) => {
      if (role === true)
        this.authSrv.changeRolee('Admin');
      else
      this.authSrv.changeRolee('User');
    });
  }

  loginAsAdmin() {
    this.isAdmin = true;
    this.login();
  }
}
