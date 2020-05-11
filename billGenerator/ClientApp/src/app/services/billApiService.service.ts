import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { billFormModel } from 'src/app/Models/billFormModel';


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


  generatePDF(formDetails: billFormModel) {  
    this._httpClient.post(this.baseUrl + "api/Bill/generatePdfsad", formDetails, {responseType:'text'}).subscribe();
  }
}
