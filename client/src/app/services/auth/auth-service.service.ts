import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginRequestt } from '../../models/loginRequestt.model';
import { User } from '../../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {
  AUTH_URL = 'http://localhost:5127/api/AuthUser';
  constructor() { }
  http: HttpClient = inject(HttpClient);
  isAdminn: string = ""

  login(details: LoginRequestt): Observable<any> {
    return this.http.post<string>(this.AUTH_URL+"/login",details);
  }

  register(details: User): Observable<any> {
    return this.http.post<string>(this.AUTH_URL+"/register",details);
  }
  
  getRole():Observable<boolean>{
    const token=localStorage.getItem('token')
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}` )
    return this.http.get<boolean>(this.AUTH_URL+"/getRole",{ headers })
  }
  changeRolee(rolee:string){
    this.isAdminn=rolee
   }
   
}
