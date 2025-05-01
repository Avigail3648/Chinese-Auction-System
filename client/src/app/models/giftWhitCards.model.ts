import { Card } from "./card.model";
import { Gift } from "./gift.model";

export class GiftWithCards {
    gift: Gift = new Gift();
    cards: Card[] = []
}