import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MeterReadingListComponent } from './meter-reading-list/meter-reading-list.component';
import { AccountListComponent } from './account-list/account-list.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AccountCreateFormComponent } from './account-list/account-create-form/account-create-form.component';
import { MeterReadingFormComponent } from './meter-reading-list/meter-reading-form/meter-reading-form.component';

@NgModule({
  declarations: [
    AppComponent,
    MeterReadingListComponent,
    AccountListComponent,
    PageNotFoundComponent,
    AccountCreateFormComponent,
    MeterReadingFormComponent
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    NgbModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
