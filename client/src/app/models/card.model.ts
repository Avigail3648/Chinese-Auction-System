import { Gift } from "./gift.model"
import { User } from "./user.model"

export class Card{
    cardId :number=0
    gift :Gift=new Gift()
    user :User=new User()
    isWin :boolean=false
  }
 