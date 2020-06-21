import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';

import { BillFormComponent } from './bill-form/bill-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule, MatCardModule, MatProgressSpinnerModule, MatDatepickerModule, MatNativeDateModule, MatSnackBarModule } from '@angular/material';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { billApiService } from 'src/app/services/billApiService.service';
import { MatIconModule } from '@angular/material/icon';
import { saveAs } from 'file-saver';
import { BillComponent } from './bill/bill.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    BillFormComponent,
    BillComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: 'billForm', component: BillFormComponent},
      { path: '', redirectTo:'billForm',pathMatch: 'full' },
      { path: 'bill', component: BillComponent}
    ]),
    BrowserAnimationsModule,
    MatButtonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule,
    MatIconModule
  ],
  providers: [billApiService],
  bootstrap: [AppComponent],
})
export class AppModule { }
