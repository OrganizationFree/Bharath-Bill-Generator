import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators, } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { billApiService } from 'src/app/services/billApiService.service';
import { billFormModel } from '../Models/billFormModel';
import { MatSnackBar } from '@angular/material/snack-bar';

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
      'Price': [null, Validators.required]
      //,'GrandTotal': ['']
      , 'Tax': ['']
      , 'Total': ['']
      //,'AmountInWords': ['']
      , 'ClientName': [null, Validators.required]
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

  constructor(private _fb: FormBuilder, private _http: HttpClient, @Inject('BASE_URL') baseUrl: string,
    private _billApiService: billApiService, private _snackBar: MatSnackBar) {
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    this.initForm();

    this.billFormGroup.controls['CGST'].setValue(0);
    this.billFormGroup.controls['SGST'].setValue(0);
    this.billFormGroup.controls['IGST'].setValue(0);
    this.billFormGroup.controls['Weight'].setValue(0);
    this.billFormGroup.controls['Rate'].setValue(0);
    this.billFormGroup.controls['Price'].setValue(0);
    this.billFormGroup.controls['Tax'].setValue(0);
    this.billFormGroup.controls['Total'].setValue(0);




    this.billFormGroup.controls['CGST'].valueChanges.subscribe(value => {
      this.calculatePrice();
      this.billFormGroup.controls['SGST'].setValue(this.billFormGroup.controls['CGST'].value);
      if (this.billFormGroup.controls['IGST'].value != 0 && this.billFormGroup.controls['CGST'].value != 0)
        this.billFormGroup.controls['IGST'].setValue(0);
    });

    this.billFormGroup.controls['IGST'].valueChanges.subscribe(value => {
      this.calculatePrice();
      if (this.billFormGroup.controls['IGST'].value != 0 && this.billFormGroup.controls['CGST'].value != 0)
        this.billFormGroup.controls['CGST'].setValue(0);
    });

    this.billFormGroup.controls['Weight'].valueChanges.subscribe(value => {
      this.calculatePrice();
    });

    this.billFormGroup.controls['Rate'].valueChanges.subscribe(value => {
      this.calculatePrice();
    });



  }
  calculatePrice() {
    let weight = this.billFormGroup.controls['Weight'].value;
    let rate = this.billFormGroup.controls['Rate'].value;
    let price = (weight * rate);
    this.billFormGroup.controls['Price'].setValue(price);
    let GST = this.billFormGroup.controls['CGST'].value != 0 ? this.billFormGroup.controls['CGST'].value * 2 : this.billFormGroup.controls['IGST'].value;
    let tax = (GST * this.billFormGroup.controls['Price'].value) / 100;
    this.billFormGroup.controls['Tax'].setValue(tax);
    let total = (+price + +tax);
    this.billFormGroup.controls['Total'].setValue(total);
  }

  generatepdf() {

    if (this.billFormGroup.valid) {
      let test = this._billApiService.generatePDF(this.billFormGroup.value);
      console.log('gene test : ',test);
    }
    else {
      this.billFormGroup.markAllAsTouched();
      this.showSnackBar('Please fill the required field with valid data and try again.', 'OK');
    }
    //console.log(this.billFormGroup.value);
  }




  showSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 3000,
    });
  }



}
