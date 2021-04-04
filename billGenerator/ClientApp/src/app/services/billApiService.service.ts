import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { billFormModel } from 'src/app/Models/billFormModel';
import { saveAs } from 'file-saver';

@Injectable({
providedIn:'root'
}
)
export class billApiService {

  billFormObject: billFormModel;
  baseUrl: string;

  constructor(private _httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  generateInvoice(formDetails: billFormModel): any {
    var mediaType = 'application/pdf';
    this._httpClient.post(this.baseUrl + "api/Bill/generateInvoice", formDetails, { responseType: 'blob' }).subscribe(
      (response) => {
        var blob = new Blob([response], { type: mediaType });
        saveAs(blob, 'NMC_Bill.pdf');
      },
      e => {
         //throwError(e);
      }
    );
  }

  generateEstimate(formDetails: billFormModel): any {
    var mediaType = 'application/pdf';
    this._httpClient.post(this.baseUrl + "api/Bill/generateEstimate", formDetails, { responseType: 'blob' }).subscribe(
      (response) => {
        var blob = new Blob([response], { type: mediaType });
        saveAs(blob, 'NMC_Bill.pdf');
      },
      e => {
        //throwError(e);
      }
    );
  }



}
