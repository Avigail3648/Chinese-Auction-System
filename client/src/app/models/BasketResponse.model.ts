import { Gift } from "./gift.model"

export class BasketResponse{
    basketId:number=0
    gift:Gift=new Gift()
    userId:number=0
}