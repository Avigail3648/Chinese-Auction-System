import { Component, inject, OnInit } from '@angular/core';
import { AuthServiceService } from '../../services/auth/auth-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navigator',
  templateUrl: './navigator.component.html',
  styleUrls: ['./navigator.component.css']
})
export class NavigatorComponent implements OnInit {
  isAdmin: boolean = false; 
  isNot: boolean = true;
  router: Router = inject(Router);

  constructor(private authService: AuthServiceService) {}

  ngOnInit(): void {
    this.authService.getRole().subscribe((role) => {
      this.isAdmin = role;
    });
  }

  navigate(s: string) {
    this.router.navigate([`/${s}`]);
  }
}
