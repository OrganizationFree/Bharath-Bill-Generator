import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators,FormArray } from '@angular/forms';
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
      'Tax': [''],
      'GrandTotal': [''],
      'TotalPrice':['0'],
      'ClientName': [null, Validators.required],
      'ClientAddress': [null, Validators.required],
      'BillDate': [null, Validators.required],
      'NoOfArticles': [null, Validators.required],
      'CGST': [null, Validators.required],
      'SGST': [null, Validators.required],
      'IGST': [null, Validators.required],
      'GSTIN': [null,[Validators.required, Validators.pattern("^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[A-Z0-9]{1}[Z]{1}[A-Z0-9]{1}$")]],
      'Transport': [null],
      Items: this._fb.array([
        this.addNewItem()
        ])
    });
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
    this.billFormGroup.controls['Tax'].setValue(0);
    this.billFormGroup.controls['GrandTotal'].setValue(0);
    this.billFormGroup.controls['TotalPrice'].setValue(0);

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

    this.billFormGroup.controls['Items'].valueChanges.subscribe(value => {
      this.calculatePrice();
    });

    //this.billFormGroup.controls['Items'].valueChanges.subscribe(value => {
    //  this.calculatePrice();
    //});

  }

  generateNewItem(): void {
    (<FormArray>this.billFormGroup.get('Items')).push(this.addNewItem());
  }

  deleteThisItem(itemIndex: number): void {
    (<FormArray>this.billFormGroup.get('Items')).removeAt(itemIndex);
  }

  addNewItem() : FormGroup {
    return  this._fb.group({
      Weight: [0, Validators.required],
      Rate: [0, Validators.required],
      Price: [0, Validators.required]
    });
  }

  get Items() {
    return this.billFormGroup.get('Items') as FormArray;
  }

  calculatePrice() {
    let ItemPrice:number = 0;
    let totalPrice: number = 0;
    for (let item of this.Items.controls) {
      ItemPrice = parseFloat(item.value.Weight) * parseFloat(item.value.Rate);
      item.patchValue({ 'Price': (ItemPrice).toFixed(2) }, { emitEvent: false });
      totalPrice = (totalPrice) + (ItemPrice);
      this.billFormGroup.patchValue({ 'TotalPrice': totalPrice.toFixed(2) }, {emitEvent:false});
    }
    //Object.keys(lclItems.controls).forEach(key => {
    //  lclItems.controls[key].markAsDirty();
    //});
    let GST = this.billFormGroup.controls['CGST'].value != 0 ? this.billFormGroup.controls['CGST'].value * 2 : this.billFormGroup.controls['IGST'].value;
    let tax = (GST * this.billFormGroup.controls['TotalPrice'].value) / 100;
    this.billFormGroup.controls['Tax'].setValue(tax.toFixed(2));
    let total = (+totalPrice + +tax);
    this.billFormGroup.controls['GrandTotal'].setValue(total.toFixed(2));
  }

  generatepdf() {
    console.log(this.billFormGroup.value);
    if (this.billFormGroup.valid) {
      let test = this._billApiService.generatePDF(this.billFormGroup.value);
      console.log('gene test : ', test);
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
