import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AuthModule } from './features/auth/auth.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ConfirmEmailComponent } from './features/auth/confirm-email/confirm-email.component';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { CreateOfficeDialogComponent } from './pages/offices/create-office-dialog/create-office-dialog.component';
import { MatRadioModule } from '@angular/material/radio';
import { OfficesComponent } from './pages/offices/offices.component';
import { CancelConfirmDialogComponent } from '@shared/dialogs/cancel-confirm-dialog/cancel-confirm-dialog.component';
import { NavbarComponent } from '@shared/menu/navbar.component';
import { OfficeDetailsComponent } from './pages/offices/office-details.component';
import { HttpErrorInterceptor } from '@core/global-error-handler';
import { AuthInterceptor } from '@features/auth/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    ConfirmEmailComponent,
    CreateOfficeDialogComponent,
    OfficesComponent,
    CancelConfirmDialogComponent,
    NavbarComponent,
    OfficeDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatRadioModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
