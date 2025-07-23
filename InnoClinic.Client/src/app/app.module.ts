import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AuthModule } from './features/auth/auth.module';
import { HttpClientModule } from '@angular/common/http';
import { ConfirmEmailComponent } from './features/auth/confirm-email/confirm-email.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HomePatientComponent } from './pages/home-patient/home.patient.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';

@NgModule({
  declarations: [AppComponent, ConfirmEmailComponent, HomePatientComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatDialogModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
