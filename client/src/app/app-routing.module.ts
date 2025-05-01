import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GiftComponent } from './components/gift/gift.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { DonorComponent } from './components/donor/donor.component';
import { BasketComponent } from './components/basket/basket.component';
import { CardComponent } from './components/card/card.component';
import { SaleGiftComponent } from './components/sale-gift/sale-gift.component';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
  {path:'', component:HomeComponent},
  {path:'gift', component:GiftComponent},
  {path:'donor', component:DonorComponent},
  {path:'login', component:LoginComponent},
  {path:'register', component:RegisterComponent},
  {path:'basket', component:BasketComponent},
  {path:'card', component:CardComponent},
  {path:'giftSale', component:SaleGiftComponent},
]
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
