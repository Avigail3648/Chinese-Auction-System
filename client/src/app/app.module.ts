import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { GiftComponent } from './components/gift/gift.component';
import {NoopAnimationsModule} from '@angular/platform-browser/animations'
import {ImportsModule} from './imports';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { BasketComponent } from './components/basket/basket.component';
import { CardComponent } from './components/card/card.component';
import { DonorComponent } from './components/donor/donor.component';
import { AdminLoginComponent } from './components/admin-login/admin-login.component';
import { SaleGiftComponent } from './components/sale-gift/sale-gift.component';
import { NavigatorComponent } from './components/navigator/navigator.component';
import { HomeComponent } from './components/home/home.component';

@NgModule({

  declarations: [
    AppComponent,
    GiftComponent,
    LoginComponent,
    RegisterComponent,
    BasketComponent,
    CardComponent,
    DonorComponent,
    AdminLoginComponent,
    SaleGiftComponent,
    NavigatorComponent,
    HomeComponent 
  ],
  imports: [    
    BrowserModule,
    AppRoutingModule,
    NoopAnimationsModule,
    ImportsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
