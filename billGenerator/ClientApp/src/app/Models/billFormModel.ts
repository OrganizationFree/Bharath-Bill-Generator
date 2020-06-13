import { itemsFormModel } from "./itemsFormModel";

export class billFormModel {
  ClientName: string;
  ClientAddress: string;
  BillDate: Date;
  NoOfArticles?: number;
  CGST?: number;
  SGST?: number;
  IGST?: number;
  Tax: number;
  GSTIN: string;
  Transport: string;
  Items: itemsFormModel[];
  TotalPrice: number;
  GrandTotal: number;
  //ClientId: number;
  //Particulars: string;
  //AmountInWords: string;
}
