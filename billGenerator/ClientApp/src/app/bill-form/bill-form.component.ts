import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { billApiService } from 'src/app/services/billApiService.service';
import { billFormModel } from '../Models/billFormModel';

@Component({
  selector: 'app-bill-form',
  templateUrl: './bill-form.component.html',
  styleUrls: ['./bill-form.component.css']
})
export class BillFormComponent implements OnInit {

  billFormGroup: FormGroup;
  btnDisable: boolean = true;
  baseUrl: string;

  initForm() {
    this.billFormGroup = this._fb.group({
      //'ClientId': ['']
      //,'Particulars': ['']
      //,'Amount': ['']
      //,'GrandTotal': ['']
      //,'Tax': ['']
      //,'Total': ['']
      //,'AmountInWords': ['']
      'ClientName': [null, Validators.required]
      , 'ClientAddress': [null, Validators.required]
      , 'BillDate': [null, Validators.required]
      , 'NoOfArticles': [null, Validators.required]
      , 'Weight': [null, Validators.required]
      , 'Rate': [null, Validators.required]
      , 'CGST': [null, Validators.required]
      , 'SGST': [null, Validators.required]
      , 'IGST': [null, Validators.required]
    })
  }

  constructor(private _fb: FormBuilder, private _http: HttpClient, @Inject('BASE_URL') baseUrl: string,private _billApiService: billApiService) {
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    this.initForm();
  }

  generatepdf() {
  
    if (this.billFormGroup.valid) {        
      this._billApiService.generatePDF(this.billFormGroup.value);
    }
    else
      alert('Please fill all required fields');
    console.log(this.billFormGroup.value);
  }

 

}
