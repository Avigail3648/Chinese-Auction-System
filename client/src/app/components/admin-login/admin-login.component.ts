import { Component, EventEmitter, Output, inject } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { AuthServiceService } from '../../services/auth/auth-service.service';
import { LoginRequestt } from '../../models/loginRequestt.model';

@Component({
  selector: 'app-admin-login',
  templateUrl: './admin-login.component.html',
  providers: [MessageService],
  styleUrls: ['./admin-login.component.css']
})
export class AdminLoginComponent {
  router: Router = inject(Router);
  authSrv: AuthServiceService = inject(AuthServiceService);
  messageService: MessageService;
  touchPassword: boolean = false;
  token: string = '';
  user: LoginRequestt = new LoginRequestt();
  password: string = '';
  mail: string = '';
  isAdmin: boolean = false; 

  @Output() returnToFather: EventEmitter<boolean> = new EventEmitter<boolean>();
  AuthService: AuthServiceService = inject(AuthServiceService);

  constructor(messageService: MessageService) {
    this.messageService = messageService;
  }
  ngOnInit(): void {
    this.AuthService.getRole().subscribe((role) => {
      this.isAdmin = role; 
    });
  }

  login() {
    this.user.password = this.password;
    this.user.mail = '*@gmail.com';
    this.authSrv.login(this.user).subscribe(
      (data) => {
        this.token = data.token;
        localStorage.setItem('token', this.token);
        this.navigateBasedOnRole();  
        window.location.reload();
      },
      (error) => {
        let errorMessage = 'משתמש לא רשום';
        this.messageService.add({
          severity: 'error',
          summary: 'שגיאה',
          detail: errorMessage,
        });
      }
    );
  }

  navigateBasedOnRole() {
    this.authSrv.getRole().subscribe((role) => {
      if (role) {
        this.router.navigate(['/gift']); 
      } else {
        this.router.navigate(['/basket']); 
      }
    });
  }

  return() {
    this.returnToFather.emit(false);
  }
}
